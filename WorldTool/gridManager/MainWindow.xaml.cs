using System;
using System.Collections;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Xml.Serialization;
using System.Data;

namespace GridManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Function _fc = new Function();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Init()
        {
            chbMapOverlay.IsEnabled = false;
            chbMapOverlay.IsChecked = false;
            gdGridView.Children.Clear();
            dgGridInfo.ItemsSource = null;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Init();
        }

        private void btGridPath_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Filter = "All grid file | grid.xml";

            if ((bool)openFileDialog.ShowDialog())
            {
                try
                {
                    Stream checkStream;
                    if ((checkStream = openFileDialog.OpenFile()) != null)
                    {
                        tbGridPath.Text = openFileDialog.FileName;
                        lbEntitiesPath.Content = System.IO.Path.GetDirectoryName(openFileDialog.FileName) + @"\server_entities.xml";

                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not read file from disk." + Environment.NewLine + "Original error: " + ex.Message);
                }
            }
        }

        private void btShowMap_Click(object sender, RoutedEventArgs e)
        {
            if (tbGridPath.Text == "")
                return;

            btShowMap.IsEnabled = false;

            do
            {
                MessageBoxResult ret = MessageBox.Show("월드를 불러옵니다.", "확인", MessageBoxButton.YesNo);
                if (ret != MessageBoxResult.Yes)
                    break;

                Init();

                Grid xmlGridData = _fc.LoadGridDataFromXml(tbGridPath.Text);
                if (xmlGridData == null)
                    continue;

                if (xmlGridData.gridInfos.gridInfo.Count > 0)
                {
                    int xCount = xmlGridData.gridInfos.CountX;
                    int yCount = xmlGridData.gridInfos.CountY;

                    if (_fc._xCount != xCount || _fc._yCount != xCount)
                    {
                        _fc._xCount = xCount;
                        _fc._yCount = yCount;
                        _fc._flagWorldSizeHasChanged = true;
                    }

                    _fc._xmlDataSet = new XmlDataSet[_fc._xCount, _fc._yCount];

                    if (!(_fc.XmlGridDataToXmlDataSet(xmlGridData, lvLandformType, lvProvince_id, lvWorldSummaryInfo)))
                        break;

                    Level xmlEntityData = _fc.LoadEntityDataFromXml(lbEntitiesPath.Content.ToString());
                    if (xmlEntityData != null)
                        _fc.XmlEntityDataToXmlDataSet(xmlEntityData);

                    bool loadOverlayMapImage = _fc.LoadOverlayImage(tbGridPath.Text, imgMapViewportOverlay);

                    chbMapOverlay.IsEnabled = loadOverlayMapImage;
                    chbMapOverlay.IsChecked = loadOverlayMapImage;

                    _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);

                    ScaleTransform[] changeToDefaultScale, adjustImageSize;
                    changeToDefaultScale = new ScaleTransform[] { imgMapViewportGridScaleTransform, imgMapViewportSelectedScaleTransform };
                    adjustImageSize = new ScaleTransform[] { imgMapViewportOverlayScaleTransform };

                    _fc.ExpandImageChangeToDefaultScale(changeToDefaultScale, adjustImageSize);

                    xmlGridData = null;
                    xmlEntityData = null;

                    MessageBox.Show("완료 되었습니다.");

                }
            } while (false);

            btShowMap.IsEnabled = true;
        }

        private void btSaveMap_Click(object sender, RoutedEventArgs e)
        {
            if (_fc._xmlDataSet == null)
                return;

            try
            {
                btSaveMap.IsEnabled = false;

                MessageBoxResult ret = MessageBox.Show(
                    "수정한 내용을 저장합니다.", "확인", MessageBoxButton.YesNo);
                if (ret != MessageBoxResult.Yes)
                    return;

                Grid xmlGridData = _fc.LoadGridDataFromXml(tbGridPath.Text);

                if (xmlGridData == null)
                    return;

                for (int x = 0; x < _fc._xCount; x++)
                {
                    for (int y = 0; y < _fc._yCount; y++)
                    {
                        if (_fc._gridSelected[x, y] == false)
                            continue;

                        GridInfo gridInfo = xmlGridData.gridInfos.gridInfo[_fc._xmlDataSet[x, y].index];
                        if ((bool)chbLandformType.IsChecked)
                            gridInfo.landformType = _fc.EditGridInfoValue(tbLandformType.Text);
                        if ((bool)chbProvince_id.IsChecked)
                            gridInfo.provinceId = _fc.EditGridInfoValue(tbProvince_id.Text);
                        if ((bool)chbConstruction_allow.IsChecked)
                            gridInfo.constructionAllow = _fc.EditGridInfoValue(tbConstruction_allow.Text);
                        if ((bool)chbTimeOfDayFileIdx.IsChecked)
                            gridInfo.timeOfDayFileIdx = _fc.EditGridInfoValue(tbTimeOfDayFileIdx.Text);
                        if ((bool)chbBrickNameIdx.IsChecked)
                            gridInfo.brickNameIdx = _fc.EditGridInfoValue(tbBrickNameIdx.Text);
                        if ((bool)chbConstruction_forbid.IsChecked)
                            gridInfo.constructionForbid = _fc.EditGridInfoValue(tbConstruction_forbid.Text);
                        if ((bool)chbzone_id.IsChecked)
                            gridInfo.zoneId = _fc.EditGridInfoValue(tbzone_id.Text);
                        if ((bool)chbCityAreaExpandLevel.IsChecked)
                            gridInfo.cityAreaExpandLevel = _fc.EditGridInfoValue(tbCityAreaExpandLevel.Text);
                    }
                }

                _fc.SaveGridDataToXml(tbGridPath.Text, xmlGridData);

                Level xmlEntityData = _fc.LoadEntityDataFromXml(lbEntitiesPath.Content.ToString());
                _fc.XmlGridDataToXmlDataSet(xmlGridData, lvLandformType, lvProvince_id, lvWorldSummaryInfo);
                _fc.XmlEntityDataToXmlDataSet(xmlEntityData);
                _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);

                MessageBox.Show("저장되었습니다.");
            }
            catch
            {

            }
            finally
            {
                btSaveMap.IsEnabled = true;
            }
        }

        private void imgMapViewportSelected_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Image img = sender as Image;

                int x = (int)(e.GetPosition(img).X / _fc.GetGridSize());
                int y = _fc.InverseY((int)(e.GetPosition(img).Y / _fc.GetGridSize()));

                if (x < 0 || y < 0 || x >= _fc._xCount || y >= _fc._yCount) return;
                if (_fc._currnet.x == x && _fc._currnet.y == y) return;

                _fc._currnet.x = x;
                _fc._currnet.y = y;

                XmlDataSet xmlDataSet = _fc._xmlDataSet[x, y];
                string gridInfo =
                    "X: " + x.ToString() + " Y: " + y.ToString() + Environment.NewLine +
                    "LandformType: " + xmlDataSet.LandformType.value.ToString() + Environment.NewLine +
                    "ClimateType: " + xmlDataSet.ClimateType.value.ToString() + Environment.NewLine +
                    "TimeOfDayFileIdx: " + xmlDataSet.TimeOfDayFileIdx.value.ToString() + Environment.NewLine +
                    "BrickNameIdx: " + xmlDataSet.BrickNameIdx.value.ToString();
                lbInfo.Content = gridInfo;

                string gridInfo2 =
                    "provinceId: " + xmlDataSet.province_id.value.ToString() + Environment.NewLine +
                    "constructionAllow: " + xmlDataSet.constructionAllow.ToString() + Environment.NewLine +
                    "constructionForbid: " + xmlDataSet.constructionForbid.ToString() + Environment.NewLine +
                    "CityAreaExpandLevel: " + xmlDataSet.cityAreaExpandLevel.value.ToString();
                lbInfo2.Content = gridInfo2;

                if (Mouse.LeftButton == MouseButtonState.Pressed)
                    _fc.SelectGrid(x, y, true, 1);
                else if (Mouse.RightButton == MouseButtonState.Pressed)
                    _fc.SelectGrid(x, y, false, 1);
            }
            catch
            { }

        }

        private void imgMapViewportSelected_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _fc._currnet.x = -1;
            _fc._currnet.y = -1;

            Image img = sender as Image;
            int x = (int)(e.GetPosition(img).X / _fc.GetGridSize());
            int y = _fc.InverseY((int)(e.GetPosition(img).Y / _fc.GetGridSize()));

            if (e.LeftButton == MouseButtonState.Pressed && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                btSaveEntities.IsEnabled = false;
                tbGridX.Text = "X: " + x.ToString();
                tbGridY.Text = "Y: " + y.ToString();
                dgGridInfo.ItemsSource = null;
                gdGridView.Children.Clear();
                _fc.EntityButtonClear();

                if (_fc._xmlDataSet[x, y].entitiesList.Count > 0)
                {
                    _fc._xmlDataSet[x, y].entitiesList.Sort(new SortunitClassCompare());

                    DataTable gridEntitiesInfo = new DataTable("gridInfo");

                    DataColumn idx = new DataColumn("idx", typeof(int));
                    DataColumn name = new DataColumn("Name", typeof(string));
                    DataColumn entityClass = new DataColumn("EntityClass", typeof(string));
                    DataColumn posX = new DataColumn("PosX", typeof(double));
                    DataColumn posY = new DataColumn("PosY", typeof(double));
                    DataColumn posZ = new DataColumn("PosZ", typeof(double));
                    DataColumn contentsId = new DataColumn("contentsId", typeof(string));
                    DataColumn defaultUnitId = new DataColumn("defaultUnitId", typeof(string));
                    DataColumn nNpcBasicTableId = new DataColumn("nNpcBasicTableId", typeof(string));
                    DataColumn id = new DataColumn("id", typeof(string));

                    gridEntitiesInfo.Columns.Add(idx);
                    gridEntitiesInfo.Columns.Add(name);
                    gridEntitiesInfo.Columns.Add(entityClass);
                    gridEntitiesInfo.Columns.Add(contentsId);
                    gridEntitiesInfo.Columns.Add(defaultUnitId);
                    gridEntitiesInfo.Columns.Add(nNpcBasicTableId);
                    gridEntitiesInfo.Columns.Add(posX);
                    gridEntitiesInfo.Columns.Add(posY);
                    gridEntitiesInfo.Columns.Add(posZ);
                    gridEntitiesInfo.Columns.Add(id);

                    dgGridInfo.ItemsSource = gridEntitiesInfo.DefaultView;

                    foreach (XmlDataSet.entities entity in _fc._xmlDataSet[x, y].entitiesList)
                    {
                        DataRow entityRow = gridEntitiesInfo.NewRow();
                        entityRow[idx] = gridEntitiesInfo.Rows.Count;
                        entityRow[name] = entity.name;
                        entityRow[entityClass] = entity.entityClass;
                        entityRow[posX] = entity.posX;
                        entityRow[posY] = entity.posY;
                        entityRow[posZ] = entity.posZ;
                        if (entity.contentsId != null)
                            entityRow[contentsId] = entity.contentsId;
                        if (entity.defaultUnitId != null)
                            entityRow[defaultUnitId] = entity.defaultUnitId;
                        if (entity.nNpcBasicTableId != null)
                            entityRow[nNpcBasicTableId] = entity.nNpcBasicTableId;
                        entityRow[id] = entity.id;

                        gridEntitiesInfo.Rows.Add(entityRow);

                        Button entityButton = new Button();
                        gdGridView.Children.Add(entityButton);
                        _fc.EntityButtonAdd(entityButton);
                        entityButton.Content = entityRow[idx].ToString();
                        entityButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                        entityButton.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
                        entityButton.Width = 15;
                        entityButton.Height = 15;
                        foreach (object fieldValue in entityRow.ItemArray)
                            if (fieldValue.ToString() != string.Empty)
                                entityButton.ToolTip += fieldValue.ToString() + Environment.NewLine;
                        entityButton.Background = new SolidColorBrush(_fc.GetEntityColorEntry(entity.entityClass));
                        entityButton.Margin = new Thickness
                        (
                            (entity.posX % 64.0) * (gdGridView.Width / 64.0) - (entityButton.Width / 2),
                            0,
                            0,
                            (entity.posY % 64.0) * (gdGridView.Height / 64.0) - (entityButton.Height / 2)
                        );
                        entityButton.Click += entityButton_Click;
                    }

                }
                /*else
                {
                    dgGridInfo.ItemsSource = null;
                    gdGridView.Children.Clear();
                    _fc.entityButtonClear();
                }*/
                e.Handled = true;
            }
            else
            {
                imgMapViewportSelected_MouseMove(sender, e);
            }
        }

        void entityButton_Click(object sender, RoutedEventArgs e)
        {
            tcGridInfo.SelectedIndex = 0;
            dgGridInfo.SelectedIndex = int.Parse(((Button)sender).Content.ToString());
        }

        private void btExpandDown_Click(object sender, RoutedEventArgs e)
        {
            ScaleTransform[] changeScale = new ScaleTransform[]
            {
                imgMapViewportGridScaleTransform,
                imgMapViewportSelectedScaleTransform,
                imgMapViewportOverlayScaleTransform
            };
            _fc.ExpandImage(changeScale, 0.5);
        }

        private void btExpandUp_Click(object sender, RoutedEventArgs e)
        {
            ScaleTransform[] changeScale = new ScaleTransform[]
            {
                imgMapViewportGridScaleTransform,
                imgMapViewportSelectedScaleTransform,
                imgMapViewportOverlayScaleTransform
            };
            _fc.ExpandImage(changeScale, 2);
        }

        private void chbMapOverlay_Checked(object sender, RoutedEventArgs e)
        {
            imgMapViewportOverlay.Visibility = System.Windows.Visibility.Visible;
        }

        private void chbMapOverlay_Unchecked(object sender, RoutedEventArgs e)
        {
            imgMapViewportOverlay.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void scvMapViewport_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
                scvMapViewport.ScrollToHorizontalOffset(scvMapViewport.HorizontalOffset - e.Delta);
            else if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (e.Delta > 0)
                    btExpandUp_Click(sender, e);
                else if (e.Delta < 0)
                    btExpandDown_Click(sender, e);
            }
            else
                scvMapViewport.ScrollToVerticalOffset(scvMapViewport.VerticalOffset - e.Delta);

            e.Handled = true;
        }



        private void cbShowGridInfoByColorRectangle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_fc._xmlDataSet != null)
                _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);
        }

        private void btClearSelection_Click(object sender, RoutedEventArgs e)
        {
            if (_fc._writeableBmpSelected != null)
                _fc._writeableBmpSelected.Clear();
            _fc._gridSelected = new bool[_fc._xCount, _fc._yCount];
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            imgMapViewportOverlay.Opacity = e.NewValue;
        }
///
        private void chbDoodad_Click(object sender, RoutedEventArgs e)
        {
            _fc.SetFlagCheckbox(eEntityClass.Doodad, (bool)((CheckBox)sender).IsChecked);
            if (_fc._xmlDataSet != null)
                _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);
        }

        private void chbDoodadSpawner_Click(object sender, RoutedEventArgs e)
        {
            _fc.SetFlagCheckbox(eEntityClass.DoodadSpawner, (bool)((CheckBox)sender).IsChecked);
            if (_fc._xmlDataSet != null)
                _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);
        }

        private void chbGathering_Click(object sender, RoutedEventArgs e)
        {
            _fc.SetFlagCheckbox(eEntityClass.Gathering, (bool)((CheckBox)sender).IsChecked);
            if (_fc._xmlDataSet != null)
                _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);
        }

        private void chbHousing_Click(object sender, RoutedEventArgs e)
        {
            _fc.SetFlagCheckbox(eEntityClass.Housing, (bool)((CheckBox)sender).IsChecked);
            if (_fc._xmlDataSet != null)
                _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);
        }

        private void chbHousingSpawner_Click(object sender, RoutedEventArgs e)
        {
            _fc.SetFlagCheckbox(eEntityClass.HousingSpawner, (bool)((CheckBox)sender).IsChecked);
            if (_fc._xmlDataSet != null)
                _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);
        }

        private void chbNpcSpawner_Click(object sender, RoutedEventArgs e)
        {
            _fc.SetFlagCheckbox(eEntityClass.NpcSpawner, (bool)((CheckBox)sender).IsChecked);
            if (_fc._xmlDataSet != null)
                _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);
        }

        private void chbResourceSpawner_Click(object sender, RoutedEventArgs e)
        {
            _fc.SetFlagCheckbox(eEntityClass.ResourceSpawner, (bool)((CheckBox)sender).IsChecked);
            if (_fc._xmlDataSet != null)
                _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);
        }

        private void chbSpawnpoint_Click(object sender, RoutedEventArgs e)
        {
            _fc.SetFlagCheckbox(eEntityClass.Spawnpoint, (bool)((CheckBox)sender).IsChecked);
            if (_fc._xmlDataSet != null)
                _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
            {
                btSaveMap_Click(null, null);
                e.Handled = true;
            }

            else
                e.Handled = false;
        }

        private void btSaveEntities_Click(object sender, RoutedEventArgs e)
        {
            btSaveEntities.IsEnabled = false;

            if (_fc._xmlDataSet != null && MessageBox.Show("수정한 server_entities.xml 정보를 저장합니다.", "확인", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (dgGridInfo.Items.Count < 1)
                    return;

                Level xmlEntityData = _fc.LoadEntityDataFromXml(lbEntitiesPath.Content.ToString());

                DataTable editedEntitiesTable = ((DataView)(dgGridInfo.ItemsSource)).ToTable();

                foreach (DataRow editedEntitiesRow in editedEntitiesTable.Rows)
                {
                    foreach (ObjectInfo entityInfo in xmlEntityData.missions.mission.objects.objectInfo)
                    {
                        if (entityInfo.Id.Contains(editedEntitiesRow.Field<string>("id")))
                        {
                            string pos =
                                editedEntitiesRow.Field<double>("PosX").ToString() + "," +
                                editedEntitiesRow.Field<double>("PosY").ToString() + "," +
                                editedEntitiesRow.Field<double>("PosZ").ToString();

                            entityInfo.Name = editedEntitiesRow.Field<string>("Name");
                            entityInfo.Pos = pos;
                            entityInfo.EntityClass = editedEntitiesRow.Field<string>("EntityClass");
                            entityInfo.properties.contentsId = editedEntitiesRow.Field<string>("contentsId");
                            entityInfo.properties.defaultUnitId = editedEntitiesRow.Field<string>("defaultUnitId");
                            entityInfo.tableKeys.nNpcBasicTableId = editedEntitiesRow.Field<string>("nNpcBasicTableId");

                        }
                    }

                }

                _fc.SaveEntityDataToXml(lbEntitiesPath.Content.ToString(), xmlEntityData);

                _fc.XmlEntityDataToXmlDataSet(xmlEntityData);
                _fc.DrawGridMap(cbShowGridInfoByColorRectangle, imgMapViewportGrid, imgMapViewportSelected);
                MessageBox.Show("저장되었습니다.");
            }
            else
                btSaveEntities.IsEnabled = true;
        }

        private void dgGridInfo_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            btSaveEntities.IsEnabled = true;
        }


    }
}
