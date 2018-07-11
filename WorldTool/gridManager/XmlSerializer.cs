using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Windows.Media;

namespace GridManager
{


    public class XmlDataSet
    {
        public int index { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int constructionAllow { get; set; }
        public int constructionForbid { get; set; }
        public int zoneId { get; set; }
        public XmlValueSet cityAreaExpandLevel { get; set; }
        public XmlValueSet LandformType { get; set; }
        public XmlValueSet ClimateType { get; set; }
        public XmlValueSet TimeOfDayFileIdx { get; set; }
        public XmlValueSet BrickNameIdx { get; set; }
        public XmlValueSet province_id { get; set; }

        public ArrayList entitiesList = new ArrayList();

        public struct XmlValueSet
        {
            public int value;
            public Color color;
            public XmlValueSet(int value, Color color)
            {
                this.value = value;
                this.color = color;
            }
        }

        public struct entities
        {
            public string id;
            public string name;
            public double posX;
            public double posY;
            public double posZ;
            public string entityClass;
            public string defaultUnitId;
            public string contentsId;
            public string nNpcBasicTableId;

            public entities(string id, string name, double posX, double posY, double posZ, string entityClass, string defaultUnitId, string contentsId, string nNpcBasicTableId)
            {
                this.id = id;
                this.name = name;
                this.posX = posX;
                this.posY = posY;
                this.posZ = posZ;
                this.entityClass = entityClass;
                this.defaultUnitId = defaultUnitId;
                this.contentsId = contentsId;
                this.nNpcBasicTableId = nNpcBasicTableId;
            }
        }

    }

    public class SortunitClassCompare : IComparer, IComparer<XmlDataSet.entities>
    {
        public int Compare(XmlDataSet.entities x, XmlDataSet.entities y)
        {
            return x.entityClass.CompareTo(y.entityClass);
        }

        public int Compare(object x, object y)
        {
            return Compare((XmlDataSet.entities)x, (XmlDataSet.entities)y);
        }

    }



    [XmlRoot("Grid")]
    public class Grid
    {
        [XmlElement("TimeOfDayFiles")]
        public TimeOfDayFiles timeOfDayFiles = new TimeOfDayFiles();
        [XmlElement("BrickNames")]
        public BrickNames brickNames = new BrickNames();
        [XmlElement("GridInfos")]
        public GridInfos gridInfos = new GridInfos();
    }


    public class TimeOfDayFiles
    {
        [XmlElement("TimeOfDayFile")]
        public List<TimeOfDayFile> timeOfDayFile = new List<TimeOfDayFile>();
    }

    public class TimeOfDayFile
    {
        [XmlAttribute("Idx")]
        public int Idx { get; set; }
        [XmlAttribute("Value")]
        public string Value { get; set; }
    }

    public class BrickNames
    {
        [XmlElement("BrickName")]
        public List<BrickName> brickName = new List<BrickName>();
    }

    public class BrickName
    {
        [XmlAttribute("Idx")]
        public int Idx { get; set; }
        [XmlAttribute("Value")]
        public string Value { get; set; }
    }


    public class GridInfos
    {
        [XmlElement("GridInfo")]
        public List<GridInfo> gridInfo = new List<GridInfo>();
        [XmlAttribute("GridSize")]
        public int GridSize { get; set; }
        [XmlAttribute("Count")]
        public int Count { get; set; }
        [XmlAttribute("CountX")]
        public int CountX { get; set; }
        [XmlAttribute("CountY")]
        public int CountY { get; set; }
        [XmlAttribute("Width")]
        public int Width { get; set; }
        [XmlAttribute("Height")]
        public int Height { get; set; }
    }

    public class GridInfo
    {
        [XmlAttribute("x")]
        public int x { get; set; }
        [XmlAttribute("y")]
        public int y { get; set; }
        [XmlAttribute("ClimateType")]
        public string climateType { get; set; }
        [XmlAttribute("TimeOfDayFileIdx")]
        public string timeOfDayFileIdx { get; set; }
        [XmlAttribute("LandformType")]
        public string landformType { get; set; }
        [XmlAttribute("BrickNameIdx")]
        public string brickNameIdx { get; set; }
        [XmlAttribute("province_id")]
        public string provinceId { get; set; }
        [XmlAttribute("construction_allow")]
        public string constructionAllow { get; set; }
        [XmlAttribute("construction_forbid")]
        public string constructionForbid { get; set; }
        [XmlAttribute("zone_id")]
        public string zoneId { get; set; }
        [XmlAttribute("city_area_expand_level")]
        public string cityAreaExpandLevel { get; set; }
    }


    //server_entities.xml
    [XmlRoot("Level")]
    public class Level
    {
        [XmlElement("Missions")]
        public Missions missions = new Missions();
    }

    public class Missions
    {
        [XmlElement("Mission")]
        public Mission mission = new Mission();
    }

    public class Mission
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlElement("Objects")]
        public Objects objects = new Objects();
    }

    public class Objects
    {
        [XmlElement("Object")]
        public List<ObjectInfo> objectInfo = new List<ObjectInfo>();
    }

    public class ObjectInfo
    {
        [XmlAttribute("Type")]
        public string Type { get; set; }
        [XmlAttribute("Layer")]
        public string Layer { get; set; }
        [XmlAttribute("LayerGUID")]
        public string LayerGUID { get; set; }
        [XmlAttribute("Id")]
        public string Id { get; set; }
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("Pos")]
        public string Pos { get; set; }
        [XmlAttribute("FloorNumber")]
        public string FloorNumber { get; set; }
        [XmlAttribute("Rotate")]
        public string Rotate { get; set; }
        [XmlAttribute("ColorRGB")]
        public string ColorRGB { get; set; }
        [XmlAttribute("MatLayersMask")]
        public string MatLayersMask { get; set; }
        [XmlAttribute("OutdoorOnly")]
        public string OutdoorOnly { get; set; }
        [XmlAttribute("CastShadow")]
        public string CastShadow { get; set; }
        [XmlAttribute("LodRatio")]
        public string LodRatio { get; set; }
        [XmlAttribute("ViewDistRatio")]
        public string ViewDistRatio { get; set; }
        [XmlAttribute("GlobalInSegmentedWorld")]
        public string GlobalInSegmentedWorld { get; set; }
        [XmlAttribute("HiddenInGame")]
        public string HiddenInGame { get; set; }
        [XmlAttribute("RecvWind")]
        public string RecvWind { get; set; }
        [XmlAttribute("Bending")]
        public string Bending { get; set; }
        [XmlAttribute("RenderNearest")]
        public string RenderNearest { get; set; }
        [XmlAttribute("NoStaticDecals")]
        public string NoStaticDecals { get; set; }
        [XmlAttribute("CreatedThroughPool")]
        public string CreatedThroughPool { get; set; }
        [XmlAttribute("EntityClass")]
        public string EntityClass { get; set; }
        [XmlAttribute("Flags")]
        public string Flags { get; set; }
        [XmlElement("Properties")]
        public PropertiesInfo properties = new PropertiesInfo();
        [XmlElement("TableKeys")]
        public TableKeys tableKeys = new TableKeys();
    }


    public class PropertiesInfo
    {
        [XmlAttribute("defaultUnitId")]
        public string defaultUnitId { get; set; }
        [XmlAttribute("contentsId")]
        public string contentsId { get; set; }
        [XmlAttribute("creatorCivId")]
        public string creatorCivId { get; set; }
        [XmlAttribute("level")]
        public string level { get; set; }
        [XmlAttribute("step")]
        public string step { get; set; }
        [XmlAttribute("doodad_id")]
        public string doodad_id { get; set; }
        [XmlAttribute("cmsSpawnerId")]
        public string cmsSpawnerId { get; set; }
        [XmlAttribute("civId")]
        public string civId { get; set; }
        [XmlAttribute("bEnabled")]
        public string bEnabled { get; set; }
        [XmlAttribute("bForNewbie")]
        public string bForNewbie { get; set; }
        [XmlAttribute("bStart")]
        public string bStart { get; set; }
        [XmlAttribute("teamName")]
        public string teamName { get; set; }
        [XmlAttribute("attackable")]
        public string attackable { get; set; }
        [XmlElement("Physics")]
        public Physics physics = new Physics();

    }

    public class Physics
    {
        [XmlAttribute("Density")]
        public string Density { get; set; }
        [XmlAttribute("Mass")]
        public string Mass { get; set; }
        [XmlAttribute("bPhysicalize")]
        public string bPhysicalize { get; set; }
        [XmlAttribute("bPushableByPlayers")]
        public string bPushableByPlayers { get; set; }
        [XmlAttribute("bRigidBody")]
        public string bRigidBody { get; set; }
    }

    public class TableKeys
    {
        [XmlAttribute("nCivId")]
        public string nCivId { get; set; }
        [XmlAttribute("nNpcBasicTableId")]
        public string nNpcBasicTableId { get; set; }
        [XmlAttribute("nObjSpawnerGroupsId")]
        public string nObjSpawnerGroupsId { get; set; }
    }


}
