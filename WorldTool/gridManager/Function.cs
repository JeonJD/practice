using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Data;

namespace GridManager
{
    public enum eEntityClass { Doodad, DoodadSpawner, Gathering, Housing, HousingSpawner, NpcSpawner, ResourceSpawner, Spawnpoint };
    public enum eGridType { LandformType, ClimateType, TimeOfDayFileIdx, BrickNameIdx, Province_id };

    class Function
    {
        private const int _gridCount = 256;
        private const int _gridSize = 16;

        private WriteableBitmap _writeableBmpGrid;
        public WriteableBitmap _writeableBmpSelected;

        private static Random _rand = new Random();

        public int _xCount = _gridCount;
        public int _yCount = _gridCount;
        public bool _flagWorldSizeHasChanged = true;
        public double _overRatioX, _overRatioY;

        public bool[,] _gridSelected;
        public XmlDataSet[,] _xmlDataSet;
        public struct CurrentPosition
        {
            public int x, y;
            public CurrentPosition(int a, int b)
            {
                x = a;
                y = b;
            }
        }
        private struct Coordinate
        {
            public int x;
            public int y;
            public Coordinate(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        private struct OutlineCoordinate
        {
            public Coordinate up;
            public Coordinate down;
            public Coordinate left;
            public Coordinate right;
            public OutlineCoordinate(Coordinate center)
            {
                up.x = center.x;
                up.y = center.y + 1;
                down.x = center.x;
                down.y = center.y - 1;
                left.x = center.x - 1;
                left.y = center.y;
                right.x = center.x + 1;
                right.y = center.y;
            }
        }
        public CurrentPosition _currnet = new CurrentPosition(-1, -1);

        private enum eConstructionAllow  { INVALID, CITYHALL_ALLOW, OUTPOST_ALLOW };

        private List<Color> _gridColorEntry = new List<Color>
        {
            Colors.Transparent,
            Colors.AntiqueWhite,
            Colors.Aqua,
            Colors.Aquamarine,
            Colors.Azure,
            Colors.Beige,
            Colors.Bisque,
            Colors.BlanchedAlmond,
            Colors.BlueViolet,
            Colors.Brown,
            Colors.BurlyWood,
            Colors.CadetBlue,
            Colors.Chartreuse,
            Colors.Chocolate,
            Colors.Coral,
            Colors.CornflowerBlue,
            Colors.Cornsilk,
            Colors.Crimson,
            Colors.Cyan,
            Colors.DarkBlue,
            Colors.DarkCyan,
            Colors.DarkGoldenrod,
            Colors.DarkGray,
            Colors.DarkGreen,
            Colors.DarkKhaki,
            Colors.DarkMagenta,
            Colors.DarkOliveGreen,
            Colors.DarkOrange,
            Colors.DarkOrchid,
            Colors.DarkRed,
            Colors.DarkSalmon,
            Colors.DarkSeaGreen,
            Colors.DarkSlateBlue,
            Colors.DarkSlateGray,
            Colors.DarkTurquoise,
            Colors.DarkViolet,
            Colors.DeepPink,
            Colors.DeepSkyBlue,
            Colors.DimGray,
            Colors.DodgerBlue,
            Colors.Firebrick,
            Colors.FloralWhite,
            Colors.ForestGreen,
            Colors.Fuchsia,
            Colors.Gainsboro,
            Colors.GhostWhite,
            Colors.Gold,
            Colors.Goldenrod,
            Colors.Gray,
            Colors.Green,
            Colors.GreenYellow,
            Colors.Honeydew,
            Colors.HotPink,
            Colors.IndianRed,
            Colors.Indigo,
            Colors.Ivory,
            Colors.Khaki,
            Colors.Lavender,
            Colors.LavenderBlush,
            Colors.LawnGreen,
            Colors.LemonChiffon,
            Colors.LightBlue,
            Colors.LightCoral,
            Colors.LightCyan,
            Colors.LightGoldenrodYellow,
            Colors.LightGray,
            Colors.LightGreen,
            Colors.LightPink,
            Colors.LightSalmon,
            Colors.LightSeaGreen,
            Colors.LightSkyBlue,
            Colors.LightSlateGray,
            Colors.LightSteelBlue,
            Colors.LightYellow,
            Colors.Lime,
            Colors.LimeGreen,
            Colors.Linen,
            Colors.Magenta,
            Colors.Maroon,
            Colors.MediumAquamarine,
            Colors.MediumBlue,
            Colors.MediumOrchid,
            Colors.MediumPurple,
            Colors.MediumSeaGreen,
            Colors.MediumSlateBlue,
            Colors.MediumSpringGreen,
            Colors.MediumTurquoise,
            Colors.MediumVioletRed,
            Colors.MidnightBlue,
            Colors.MintCream,
            Colors.MistyRose,
            Colors.Moccasin,
            Colors.NavajoWhite,
            Colors.Navy,
            Colors.OldLace,
            Colors.Olive,
            Colors.OliveDrab,
            Colors.Orange,
            Colors.OrangeRed,
            Colors.Orchid,
            Colors.PaleGoldenrod,
            Colors.PaleGreen,
            Colors.PaleTurquoise,
            Colors.PaleVioletRed,
            Colors.PapayaWhip,
            Colors.PeachPuff,
            Colors.Peru,
            Colors.Pink,
            Colors.Plum,
            Colors.PowderBlue,
            Colors.Purple,
            Colors.Red,
            Colors.RosyBrown,
            Colors.RoyalBlue,
            Colors.SaddleBrown,
            Colors.Salmon,
            Colors.SandyBrown,
            Colors.SeaGreen,
            Colors.SeaShell,
            Colors.Sienna,
            Colors.Silver,
            Colors.SlateBlue,
            Colors.SlateGray,
            Colors.Snow,
            Colors.SpringGreen,
            Colors.SteelBlue,
            Colors.Tan,
            Colors.Teal,
            Colors.Thistle,
            Colors.Tomato,
            Colors.Turquoise,
            Colors.Violet,
            Colors.Wheat,
            Colors.White,
            Colors.WhiteSmoke,
            Colors.Yellow,
            Colors.YellowGreen
        };

        private List<Color> _entityColorEntry = new List<Color>
        {
             Color.FromArgb(0xFF, 0xFF ,0x00 ,0x00),
             Color.FromArgb(0xFF, 0xFF, 0xA5, 0x00),
             Color.FromArgb(0xFF, 0xFF, 0xD7, 0x00),
             Color.FromArgb(0xFF, 0xAD, 0xFF, 0x2F),
             Color.FromArgb(0xFF, 0x00, 0x80, 0x00),
             Color.FromArgb(0xFF, 0x00, 0x00, 0xFF),
             Color.FromArgb(0xFF, 0x77, 0x00, 0x55),
             Color.FromArgb(0xFF, 0xFF, 0x00, 0x66)
        };

        private List<Button> _entityButton = new List<Button>();

        private bool[] _flagCheckbox = new bool[Enum.GetNames(typeof(eEntityClass)).Length];

        public void EntityButtonAdd(Button entityButton)
        {
            _entityButton.Add(entityButton);
        }

        public void EntityButtonClear()
        {
            _entityButton.Clear();
        }


        public Color GetEntityColorEntry(string entityClass)
        {
            Color resultColor = Colors.Transparent;
            try
            {
                int entityToEnum = (int)(Enum.Parse(typeof(eEntityClass), entityClass));
                resultColor = _entityColorEntry[entityToEnum];
            }
            catch { }

            return resultColor;
        }

        public void SetFlagCheckbox(eEntityClass entity, bool isCheck)
        {
            _flagCheckbox[(int)entity] = isCheck;
        }

        public int GetGridSize()
        {
            return _gridSize;
        }

        public bool LoadOverlayImage(string gridPath, Image imgMapViewportOverlay)
        {
            try
            {
                BitmapImage MapImage = new BitmapImage();
                MapImage.BeginInit();
                MapImage.UriSource = new Uri(System.IO.Path.GetDirectoryName(gridPath) + @"\grid.bmp");
                MapImage.EndInit();
                imgMapViewportOverlay.Source = MapImage;
                _overRatioX = (_xCount * _gridSize) / MapImage.Width;
                _overRatioY = (_yCount * _gridSize) / MapImage.Height;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool SetGridTypeInXmlDataSet(string type, ref List<int> listColorIdx, out int value, out Color color)
        {
            if (type != null)
            {
                int x = int.Parse(type);
                int y = listColorIdx.IndexOf(x);

                if (y == -1)
                {
                    y = listColorIdx.Count;
                    listColorIdx.Add(x);
                }
                value = x;
                if (listColorIdx.Count > _gridColorEntry.Count)
                    color = _gridColorEntry[0];
                else
                    color = _gridColorEntry[y];
                return true;
            }
            value = -1;
            color = _gridColorEntry[0];
            return false;
        }

        public bool XmlGridDataToXmlDataSet(Grid xmlGridData, ListView lvLandformType, ListView lvProvinceid, ListView lvWorldSummaryInfo)
        {
            int index = 0;
            int[] sumOfAllow = new int[Enum.GetNames(typeof(eConstructionAllow)).Length];
            List<int> colorIndexList = new List<int>();


            lvLandformType.Items.Clear();
            lvProvinceid.Items.Clear();

            List<XmlDataSet.XmlValueSet> pIdSetList = new List<XmlDataSet.XmlValueSet>();
            List<XmlDataSet.XmlValueSet> lTypeSetList = new List<XmlDataSet.XmlValueSet>();

            foreach (GridInfo Info in xmlGridData.gridInfos.gridInfo)
            {
                try
                {
                    int outValue;
                    Color outColor;
                    XmlDataSet xmlDataSet = new XmlDataSet();
                    xmlDataSet.index = index;
                    xmlDataSet.x = Info.x;
                    xmlDataSet.y = Info.y;

                    if (SetGridTypeInXmlDataSet(Info.landformType, ref colorIndexList, out outValue, out outColor))
                        xmlDataSet.LandformType = new XmlDataSet.XmlValueSet(outValue, outColor);
                    if (SetGridTypeInXmlDataSet(Info.climateType, ref colorIndexList, out outValue, out outColor))
                        xmlDataSet.ClimateType = new XmlDataSet.XmlValueSet(outValue, outColor);
                    if (SetGridTypeInXmlDataSet(Info.timeOfDayFileIdx, ref colorIndexList, out outValue, out outColor))
                        xmlDataSet.TimeOfDayFileIdx = new XmlDataSet.XmlValueSet(outValue, outColor);
                    if (SetGridTypeInXmlDataSet(Info.brickNameIdx, ref colorIndexList, out outValue, out outColor))
                        xmlDataSet.BrickNameIdx = new XmlDataSet.XmlValueSet(outValue, outColor);
                    if (SetGridTypeInXmlDataSet(Info.provinceId, ref colorIndexList, out outValue, out outColor))
                        xmlDataSet.province_id = new XmlDataSet.XmlValueSet(outValue, outColor);
                    if (SetGridTypeInXmlDataSet(Info.cityAreaExpandLevel, ref colorIndexList, out outValue, out outColor))
                        xmlDataSet.cityAreaExpandLevel = new XmlDataSet.XmlValueSet(outValue, outColor);

                    if (Info.constructionAllow != null)
                    {
                        xmlDataSet.constructionAllow = int.Parse(Info.constructionAllow);
                        sumOfAllow[xmlDataSet.constructionAllow]++;
                    }
                    if (Info.constructionForbid != null)
                        xmlDataSet.constructionForbid = int.Parse(Info.constructionForbid);
                    if (Info.zoneId != null)
                        xmlDataSet.zoneId = int.Parse(Info.zoneId);
                    index++;

                    int currentLandformType = xmlDataSet.LandformType.value;
                    int currentProvince_id = xmlDataSet.province_id.value;

                    if (currentLandformType > 100)
                    {
                        bool flagNewLandformTypeItem = true;

                        if (lTypeSetList.Count > 0)
                            foreach (XmlDataSet.XmlValueSet lTSet in lTypeSetList)
                                if (lTSet.value == currentLandformType)
                                    flagNewLandformTypeItem = false;

                        if (flagNewLandformTypeItem)
                            lTypeSetList.Add(new XmlDataSet.XmlValueSet(int.Parse(Info.landformType), xmlDataSet.LandformType.color));

                    }

                    if (currentProvince_id > 0)
                    {
                        bool flagNewProvinceIdItem = true;

                        if (pIdSetList.Count > 0)
                            foreach (XmlDataSet.XmlValueSet pIdSet in pIdSetList)
                                if (pIdSet.value == currentProvince_id)
                                    flagNewProvinceIdItem = false;

                        if (flagNewProvinceIdItem)
                            pIdSetList.Add(new XmlDataSet.XmlValueSet(int.Parse(Info.provinceId), xmlDataSet.province_id.color));
                    }

                    _xmlDataSet[Info.x, Info.y] = xmlDataSet;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Grid.xml 파일의 데이터에 오류가 있습니다." + Environment.NewLine + "Original error: " + ex.Message);
                    return false;
                }
            }

            lTypeSetList.Sort(ListSort);
            foreach (XmlDataSet.XmlValueSet lTypeSet in lTypeSetList)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = lTypeSet.value.ToString();
                newItem.Background = new SolidColorBrush(lTypeSet.color);
                lvLandformType.Items.Add(newItem);
            }

            pIdSetList.Sort(ListSort);
            foreach (XmlDataSet.XmlValueSet pIdSet in pIdSetList)
            {
                ListBoxItem newItem = new ListBoxItem();
                newItem.Content = pIdSet.value.ToString();
                newItem.Background = new SolidColorBrush(pIdSet.color);
                lvProvinceid.Items.Add(newItem);
            }

            lvWorldSummaryInfo.Items.Clear();
            string cityhallAllowCount = "CITYHALL_ALLOW: " + sumOfAllow[(int)eConstructionAllow.CITYHALL_ALLOW].ToString();
            string outpostAllowCount = "OUTPOST_ALLOW: " + sumOfAllow[(int)eConstructionAllow.OUTPOST_ALLOW].ToString();
            lvWorldSummaryInfo.Items.Add(cityhallAllowCount);
            lvWorldSummaryInfo.Items.Add(outpostAllowCount);

            return true;
        }

        private static int ListSort(XmlDataSet.XmlValueSet A, XmlDataSet.XmlValueSet B)
        {
            if (A.value > B.value) return 1;
            else if (A.value < B.value) return -1;
            return 0;
        }

        public bool XmlEntityDataToXmlDataSet(Level xmlEntityData)
        {
            try
            {
                foreach (XmlDataSet xmlDataSet in _xmlDataSet)
                    xmlDataSet.entitiesList.Clear();

                foreach (ObjectInfo objectInfo in xmlEntityData.missions.mission.objects.objectInfo)
                {
                    if (objectInfo.EntityClass == null || objectInfo.Pos == null)
                        continue;

                    string[] pos = objectInfo.Pos.ToString().Split(',');
                    double PosX = double.Parse(pos[0]);
                    double PosY = double.Parse(pos[1]);
                    double PosZ = double.Parse(pos[2]);

                    if ((int)(PosX / 64) > _xCount || (int)(PosY / 64) > _yCount)
                    {
                        MessageBox.Show("Entities.xml 파일에 그리드 값을 벗어나는 값이 있습니다." + Environment.NewLine + PosX.ToString() + Environment.NewLine + PosY.ToString());
                        continue;
                    }

                    _xmlDataSet[(int)(PosX / 64), (int)(PosY / 64)].entitiesList.Add(new XmlDataSet.entities
                    (
                        objectInfo.Id,
                        objectInfo.Name,
                        PosX,
                        PosY,
                        PosZ,
                        objectInfo.EntityClass,
                        objectInfo.properties.defaultUnitId,
                        objectInfo.properties.contentsId,
                        objectInfo.tableKeys.nNpcBasicTableId
                    ));

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Entities.xml 파일의 데이터에 오류가 있습니다." + Environment.NewLine + "Original error: " + ex.Message);
                return false;
            }

            return true;

        }

        public Grid LoadGridDataFromXml(string gridFilePath)
        {
            Grid xmlData = new Grid();

            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Grid));
                TextReader reader = new StreamReader(gridFilePath);
                object objXmlData = deserializer.Deserialize(reader);
                xmlData = (Grid)objXmlData;
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Grid 정보 파일을 읽어오는데 실패하였습니다." + Environment.NewLine + "Original error: " + ex.Message);
                return null;
            }

            return xmlData;
        }

        public Level LoadEntityDataFromXml(string entityFilePath)
        {
            Level xmlData = new Level();

            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Level));
                TextReader reader = new StreamReader(entityFilePath);
                object objXmlData = deserializer.Deserialize(reader);
                xmlData = (Level)objXmlData;
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Entity 정보 파일을 읽어오는데 실패하였습니다." + Environment.NewLine + "Original error: " + ex.Message);
                xmlData = null;
            }

            return xmlData;
        }

        public void SaveGridDataToXml(string gridFilePath, Grid xmlGridData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Grid));
            try
            {
                XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                TextWriter writer = new StreamWriter(gridFilePath);
                serializer.Serialize(writer, (object)xmlGridData, emptyNs);
                writer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Grid file path is invalid." + Environment.NewLine + "Original error: " + ex.Message);
            }

        }

        public void SaveEntityDataToXml(string EntityFilePath, Level xmlEntityData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Level));
            try
            {
                XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                TextWriter writer = new StreamWriter(EntityFilePath);
                serializer.Serialize(writer, (object)xmlEntityData, emptyNs);
                writer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Grid file path is invalid." + Environment.NewLine + "Original error: " + ex.Message);
            }

        }

        public void DrawGridMap(ComboBox cbShowGridInfoByColorRectangle, Image imgMapViewportGrid, Image imgMapViewportSelected)
        {
            try
            {
                if (_flagWorldSizeHasChanged)
                {
                    _writeableBmpGrid = BitmapFactory.New(_xCount * _gridSize, _yCount * _gridSize);
                    _writeableBmpSelected = BitmapFactory.New(_xCount * _gridSize, _yCount * _gridSize);
                    _flagWorldSizeHasChanged = false;
                }
                else
                {
                    _writeableBmpGrid.Clear();
                    _writeableBmpSelected.Clear();
                }
                imgMapViewportGrid.Source = _writeableBmpGrid;
                imgMapViewportSelected.Source = _writeableBmpSelected;
                _gridSelected = new bool[_xCount, _yCount];

                foreach (XmlDataSet xmlData in _xmlDataSet)
                {
                    int pixelX = xmlData.x * _gridSize;
                    int pixelY = InverseY(xmlData.y) * _gridSize;
                    Color backColor = Colors.Black, lineColor = Colors.Black;
                    OutlineCoordinate co = new OutlineCoordinate(new Coordinate(xmlData.x, xmlData.y));

                    switch (cbShowGridInfoByColorRectangle.SelectedIndex)
                    {

                        case 0: //case "None"
                            DrawGridMapDefaultLine(_writeableBmpGrid, xmlData.LandformType.value, pixelX, pixelY, Colors.Transparent);
                            break;

                        case 1: //case "LandformType":

                            if (xmlData.LandformType.value > 100 && xmlData.LandformType.value < 230)
                            {
                                backColor = xmlData.LandformType.color;
                            }
                            else
                            {
                                switch (xmlData.LandformType.value)
                                {
                                    case 1:
                                        backColor = Colors.SkyBlue;
                                        break;
                                    case 0:
                                        backColor = Colors.Blue;
                                        break;
                                }
                            }

                            DrawGridMapDefaultLine(_writeableBmpGrid, xmlData.LandformType.value, pixelX, pixelY, backColor);

                            if (xmlData.x > 0 && xmlData.LandformType.value != _xmlDataSet[co.left.x, co.left.y].LandformType.value)
                                DrawGridLineLeft(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y > 0 && xmlData.LandformType.value != _xmlDataSet[co.down.x, co.down.y].LandformType.value)
                                DrawGridLineDown(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.x < _xCount - 1 && xmlData.LandformType.value != _xmlDataSet[co.right.x, co.right.y].LandformType.value)
                                DrawGridLineRight(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y < _yCount -1 && xmlData.LandformType.value != _xmlDataSet[co.up.x, co.up.y].LandformType.value)
                                DrawGridLineUp(_writeableBmpGrid, pixelX, pixelY);
                            break;

                        case 2: //case "province_id":

                            DrawGridMapDefaultLine(_writeableBmpGrid, xmlData.LandformType.value, pixelX, pixelY, xmlData.province_id.color);

                            if (xmlData.x > 0 && xmlData.province_id.value != _xmlDataSet[co.left.x, co.left.y].province_id.value)
                                DrawGridLineLeft(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y > 0 && xmlData.province_id.value != _xmlDataSet[co.down.x, co.down.y].province_id.value)
                                DrawGridLineDown(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.x < _xCount - 1 && xmlData.province_id.value != _xmlDataSet[co.right.x, co.right.y].province_id.value)
                                DrawGridLineRight(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y < _yCount -1 && xmlData.province_id.value != _xmlDataSet[co.up.x, co.up.y].province_id.value)
                                DrawGridLineUp(_writeableBmpGrid, pixelX, pixelY);

                            if (xmlData.constructionAllow == (int)eConstructionAllow.CITYHALL_ALLOW)
                                _writeableBmpGrid.FillEllipse(pixelX + 1, pixelY + 1, pixelX + _gridSize - 2, pixelY + _gridSize - 2, Colors.Red);
                            else if (xmlData.constructionAllow == (int)eConstructionAllow.OUTPOST_ALLOW)
                                _writeableBmpGrid.FillEllipse(pixelX + 1, pixelY + 1, pixelX + _gridSize - 2, pixelY + _gridSize - 2, Colors.Blue);
                            else if (xmlData.constructionAllow > (int)eConstructionAllow.OUTPOST_ALLOW)
                                _writeableBmpGrid.FillEllipse(pixelX + 1, pixelY + 1, pixelX + _gridSize - 2, pixelY + _gridSize - 2, Colors.Black);
                            break;

                        case 3: //case "TimeOfDayFileIdx"

                            DrawGridMapDefaultLine(_writeableBmpGrid, xmlData.LandformType.value, pixelX, pixelY, xmlData.TimeOfDayFileIdx.color);

                            if (xmlData.x > 0 && xmlData.TimeOfDayFileIdx.value != _xmlDataSet[co.left.x, co.left.y].TimeOfDayFileIdx.value)
                                DrawGridLineLeft(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y > 0 && xmlData.TimeOfDayFileIdx.value != _xmlDataSet[co.down.x, co.down.y].TimeOfDayFileIdx.value)
                                DrawGridLineDown(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.x < _xCount - 1 && xmlData.TimeOfDayFileIdx.value != _xmlDataSet[co.right.x, co.right.y].TimeOfDayFileIdx.value)
                                DrawGridLineRight(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y < _yCount -1 && xmlData.TimeOfDayFileIdx.value != _xmlDataSet[co.up.x, co.up.y].TimeOfDayFileIdx.value)
                                DrawGridLineUp(_writeableBmpGrid, pixelX, pixelY);
                            break;

                        case 4: //case "BrickNameIdx"

                            DrawGridMapDefaultLine(_writeableBmpGrid, xmlData.LandformType.value, pixelX, pixelY, xmlData.BrickNameIdx.color);

                            if (xmlData.x > 0 && xmlData.BrickNameIdx.value != _xmlDataSet[co.left.x, co.left.y].BrickNameIdx.value)
                                DrawGridLineLeft(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y > 0 && xmlData.BrickNameIdx.value != _xmlDataSet[co.down.x, co.down.y].BrickNameIdx.value)
                                DrawGridLineDown(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.x < _xCount - 1 && xmlData.BrickNameIdx.value != _xmlDataSet[co.right.x, co.right.y].BrickNameIdx.value)
                                DrawGridLineRight(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y < _yCount -1 && xmlData.BrickNameIdx.value != _xmlDataSet[co.up.x, co.up.y].BrickNameIdx.value)
                                DrawGridLineUp(_writeableBmpGrid, pixelX, pixelY);
                            break;

                        case 5: //case "construction_forbid"

                            if (xmlData.constructionForbid <= 0)
                                backColor = Colors.Transparent;

                            DrawGridMapDefaultLine(_writeableBmpGrid, xmlData.LandformType.value, pixelX, pixelY, backColor);

                            if (xmlData.x > 0 && xmlData.constructionForbid != _xmlDataSet[co.left.x, co.left.y].constructionForbid)
                                DrawGridLineLeft(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y > 0 && xmlData.constructionForbid != _xmlDataSet[co.down.x, co.down.y].constructionForbid)
                                DrawGridLineDown(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.x < _xCount - 1 && xmlData.constructionForbid != _xmlDataSet[co.right.x, co.right.y].constructionForbid)
                                DrawGridLineRight(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y < _yCount - 1 && xmlData.constructionForbid != _xmlDataSet[co.up.x, co.up.y].constructionForbid)
                                DrawGridLineUp(_writeableBmpGrid, pixelX, pixelY);

                            if (xmlData.constructionAllow == (int)eConstructionAllow.CITYHALL_ALLOW)
                                _writeableBmpGrid.FillEllipse(pixelX + 1, pixelY + 1, pixelX + _gridSize - 2, pixelY + _gridSize - 2, Colors.Red);
                            else if (xmlData.constructionAllow == (int)eConstructionAllow.OUTPOST_ALLOW)
                                _writeableBmpGrid.FillEllipse(pixelX + 1, pixelY + 1, pixelX + _gridSize - 2, pixelY + _gridSize - 2, Colors.Blue);
                            else if (xmlData.constructionAllow > (int)eConstructionAllow.OUTPOST_ALLOW)
                                _writeableBmpGrid.FillEllipse(pixelX + 1, pixelY + 1, pixelX + _gridSize - 2, pixelY + _gridSize - 2, Colors.Black);
                            break;

                        case 6: // case "cityAreaExpandLevel"

                            DrawGridMapDefaultLine(_writeableBmpGrid, xmlData.cityAreaExpandLevel.value, pixelX, pixelY, xmlData.cityAreaExpandLevel.color);

                            if (xmlData.x > 0 && xmlData.cityAreaExpandLevel.value != _xmlDataSet[co.left.x, co.left.y].cityAreaExpandLevel.value)
                                DrawGridLineLeft(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y > 0 && xmlData.cityAreaExpandLevel.value != _xmlDataSet[co.down.x, co.down.y].cityAreaExpandLevel.value)
                                DrawGridLineDown(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.x < _xCount - 1 && xmlData.cityAreaExpandLevel.value != _xmlDataSet[co.right.x, co.right.y].cityAreaExpandLevel.value)
                                DrawGridLineRight(_writeableBmpGrid, pixelX, pixelY);
                            if (xmlData.y < _yCount -1 && xmlData.cityAreaExpandLevel.value != _xmlDataSet[co.up.x, co.up.y].cityAreaExpandLevel.value)
                                DrawGridLineUp(_writeableBmpGrid, pixelX, pixelY);

                            if (xmlData.constructionAllow == (int)eConstructionAllow.CITYHALL_ALLOW)
                                _writeableBmpGrid.FillEllipse(pixelX + 1, pixelY + 1, pixelX + _gridSize - 2, pixelY + _gridSize - 2, Colors.Red);
                            else if (xmlData.constructionAllow == (int)eConstructionAllow.OUTPOST_ALLOW)
                                _writeableBmpGrid.FillEllipse(pixelX + 1, pixelY + 1, pixelX + _gridSize - 2, pixelY + _gridSize - 2, Colors.Blue);
                            else if (xmlData.constructionAllow > (int)eConstructionAllow.OUTPOST_ALLOW)
                                _writeableBmpGrid.FillEllipse(pixelX + 1, pixelY + 1, pixelX + _gridSize - 2, pixelY + _gridSize - 2, Colors.Black);
                            break;

                        default:
                            break;
                    }

                    if (xmlData.entitiesList.Count > 0)
                    {

                        foreach (XmlDataSet.entities entity in xmlData.entitiesList)
                        {
                            int positionX1, positionY1, positionX2, positionY2;

                            try
                            {
                                int entityToEnum = (int)(Enum.Parse(typeof(eEntityClass), entity.entityClass));
                                if (_flagCheckbox[entityToEnum])
                                {
                                    GetMarkPoint(entityToEnum, pixelX, pixelY, out positionX1, out positionY1, out positionX2, out positionY2);
                                    _writeableBmpGrid.FillEllipse(positionX1, positionY1, positionX2, positionY2, _entityColorEntry[entityToEnum]);
                                }
                            }
                            catch { }
                        }
                    }
                }//foreach (XmlDataSet xmlData in _xmlDataSet)
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "XML파일의 정보를 이미지로 표현하는데 실패하였습니다." +
                    Environment.NewLine + "Original error: " + ex.Message);
            }
            finally
            {
                GC.Collect();
            }
        }

        private void DrawGridMapDefaultLine(WriteableBitmap writeableBmpGrid, int LandformType, int pixelX, int pixelY, Color backColor)
        {
            Color lineColor = Colors.Black;

            if (LandformType <= 100)
                lineColor = Colors.SkyBlue;

            writeableBmpGrid.DrawRectangle(pixelX + 1, pixelY + 1, pixelX + _gridSize - 1, pixelY + _gridSize - 1, lineColor);
            writeableBmpGrid.FillRectangle(pixelX + 1, pixelY + 1, pixelX + _gridSize - 1, pixelY + _gridSize - 1, backColor);
        }

        private void DrawGridLineUp(WriteableBitmap writeableBmpGrid, int pixelX, int pixelY)
        {
            writeableBmpGrid.DrawLineAa(pixelX, pixelY, pixelX + _gridSize - 1, pixelY, Colors.Red, 1);
        }

        private void DrawGridLineDown(WriteableBitmap writeableBmpGrid, int pixelX, int pixelY)
        {
            writeableBmpGrid.DrawLineAa(pixelX, pixelY + _gridSize - 1, pixelX + _gridSize - 1, pixelY + _gridSize - 1, Colors.Red, 1);
        }

        private void DrawGridLineLeft(WriteableBitmap writeableBmpGrid, int pixelX, int pixelY)
        {
            writeableBmpGrid.DrawLineAa(pixelX, pixelY, pixelX, pixelY + _gridSize - 1, Colors.Red, 1);
        }

        private void DrawGridLineRight(WriteableBitmap writeableBmpGrid, int pixelX, int pixelY)
        {
            writeableBmpGrid.DrawLineAa(pixelX + _gridSize - 1, pixelY, pixelX + _gridSize - 1, pixelY + _gridSize - 1, Colors.Red, 1);
        }

        public void SelectGrid(int x, int y, bool selected, int brushSize)
        {
            _gridSelected[x, y] = selected;
            x = x * _gridSize;
            y = InverseY(y) * _gridSize;

            if (selected)
            {
                _writeableBmpSelected.FillRectangle(x, y, x + _gridSize, y + _gridSize, Colors.Red);
                _writeableBmpSelected.DrawRectangle(x, y, x + _gridSize - 1, y + _gridSize - 1, Colors.Red);
            }

            else
            {
                _writeableBmpSelected.FillRectangle(x, y, x + _gridSize, y + _gridSize, Colors.Transparent);
                _writeableBmpSelected.DrawRectangle(x, y, x + _gridSize - 1, y + _gridSize - 1, Colors.Transparent);
            }

        }

        public int InverseY(int y)
        {
            return (_yCount - y) - 1;
        }

        public string EditGridInfoValue(string column)
        {
            string result = column.Trim();
            if (result == "" || result == "0") // null 상태로 XML serializatin이 되면 값을 생략,
                result = null;

            return result;
        }

        public void ExpandImage(ScaleTransform[] changeScale, double ratio)
        {
            foreach (ScaleTransform imageScale in changeScale)
            {
                imageScale.ScaleX *= ratio;
                imageScale.ScaleY *= ratio;
            }
        }

        public void ExpandImageChangeToDefaultScale(ScaleTransform[] changeToDefaultScale, ScaleTransform[] adjustImageSize)
        {
            foreach (ScaleTransform imageScale in changeToDefaultScale)
            {
                imageScale.ScaleX = 1;
                imageScale.ScaleY = 1;
            }
            foreach (ScaleTransform imageScale in adjustImageSize)
            {
                imageScale.ScaleX = _overRatioX;
                imageScale.ScaleY = _overRatioY;
            }
        }

        public void GetMarkPoint(int symbolPosition, int x, int y, out int x1, out int y1, out int x2, out int y2)
        {
            int symbolSize = _gridSize / 3;

            switch (symbolPosition)
            {
                case 0:
                    x1 = x + 1;
                    y1 = y + 1;
                    x2 = x + 1 + symbolSize;
                    y2 = y + 1 + symbolSize;
                    break;
                case 1:
                    x1 = x + symbolSize;
                    y1 = y + 1;
                    x2 = x + (symbolSize * 2);
                    y2 = y + 1 + symbolSize;
                    break;
                case 2:
                    x1 = x - 1 + (symbolSize * 2);
                    y1 = y + 1;
                    x2 = x - 1 + (symbolSize * 3);
                    y2 = y + 1 + symbolSize;
                    break;
                case 3:
                    x1 = x + 1;
                    y1 = y + symbolSize;
                    x2 = x + 1 + symbolSize;
                    y2 = y + (symbolSize * 2);
                    break;
                case 4:
                    x1 = x + symbolSize;
                    y1 = y + symbolSize;
                    x2 = x + (symbolSize * 2);
                    y2 = y + (symbolSize * 2);
                    break;
                case 5:
                    x1 = x - 1 + (symbolSize * 2);
                    y1 = y + symbolSize;
                    x2 = x - 1 + (symbolSize * 3);
                    y2 = y + (symbolSize * 2);
                    break;
                case 6:
                    x1 = x + 1;
                    y1 = y - 1 + (symbolSize * 2);
                    x2 = x + 1 + symbolSize;
                    y2 = y - 1 + (symbolSize * 3);
                    break;
                case 7:
                    x1 = x + symbolSize;
                    y1 = y - 1 + (symbolSize * 2);
                    x2 = x + (symbolSize * 2);
                    y2 = y - 1 + (symbolSize * 3);
                    break;
                case 8:
                    x1 = x - 1 + (symbolSize * 2);
                    y1 = y - 1 + (symbolSize * 2);
                    x2 = x - 1 + (symbolSize * 3);
                    y2 = y - 1 + (symbolSize * 3);
                    break;
                default:
                    x1 = x;
                    y1 = y;
                    x2 = x;
                    y2 = y;
                    break;

            }
            //x2 -= 1;
            //y2 -= 1;

        }



    }
}
