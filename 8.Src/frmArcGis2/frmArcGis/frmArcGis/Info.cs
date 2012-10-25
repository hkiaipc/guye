using System;
using System.Data;
using System.Data.OleDb ;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using System.Text;
namespace btGR
{
	/// <summary>
	/// include LayerInfo m_layers[]
	/// </summary>
	public class Info
	{
		private OleDbConnection conn=null;
		private string sql="provider=Microsoft.Jet.OLEDB.4.0;Data Source=.\\mapLayers.mdb";
		public LayerInfo[] m_layers;

        /// <summary>
        /// 
        /// </summary>
		public Info()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
//			sql=sql+"mapLayers.mdb";
			conn=new OleDbConnection(sql);
		}

		#region mapInfo
        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
		public double CalcScale(AxMapObjects2.AxMap map)
		{
			System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd((System.IntPtr)map.hWnd);
			MPoint[] pts = new MPoint[2];
			pts[0] = new MPoint();
			pts[0].x = map.Extent.Left;
			pts[0].y = map.Extent.Bottom;
			pts[1] = new MPoint();
			pts[1].x = map.Extent.Right;
			pts[1].y = map.Extent.Top;
			double Len1=pts[1].x-pts[0].x;
			double Len2=pts[1].y-pts[0].y;
			return Len1*Len2;

            #region ...
//			System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd((System.IntPtr)map.hWnd);
//
//			MPoint[] pts = new MPoint[2];
//			pts[0] = new MPoint();
//			pts[0].x = map.Extent.Left;
//			pts[0].y = map.Extent.Top;
//			pts[1] = new MPoint();
//			pts[1].x = map.Extent.Right;
//			pts[1].y = map.Extent.Top;
//  
//			double dLen1 = this.CalcLenght(pts,2);
// 
//			double dLen2 = map.Width / g.DpiX * 2.54 /100;
// 
//			return dLen1 / dLen2;
            #endregion //...
		}

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="nSize"></param>
        /// <returns></returns>
		private double CalcLenght(MPoint[] pt,int nSize)
		{
			double dLength = 0;
			double x1=0,x2=0,y1=0,y2=0;
			int nCenterL = ((int)(pt[0].x)/6+1)*6-3; 

			for(int i=0;i<nSize-1;i++)
			{
				CalGuassFromLB(pt[i].x, pt[i].y, ref x1, ref y1, nCenterL);
				CalGuassFromLB(pt[i+1].x, pt[i+1].y, ref x2, ref y2, nCenterL);
				dLength += Math.Sqrt((x1-x2)*(x1-x2)+(y1-y2)*(y1-y2));
			}

			return dLength;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dLongitude"></param>
        /// <param name="dLatitude"></param>
        /// <param name="dX"></param>
        /// <param name="dY"></param>
        /// <param name="nCenterL"></param>
		private void CalGuassFromLB(double dLongitude, double dLatitude, ref double dX, ref double dY, long nCenterL) 
		{
			
			int CenterL = (int)nCenterL;

			SubGussFs(ref dX,ref dY,dLatitude,dLongitude,CenterL);

			nCenterL = (long)CenterL;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="B"></param>
        /// <param name="L"></param>
        /// <param name="nCenterLongi"></param>
		private void SubGussFs(ref double X,ref double Y,double B,double L,int nCenterLongi)
		{
			int nzonenum;
			if(nCenterLongi==0)
			{
				nzonenum = (int)L/6+1;
				nCenterLongi = nzonenum*6-3;
			}
			else
				nzonenum = (int)nCenterLongi/6+1;

			double rB = B/180*3.1415926;
			double rL = (L-nCenterLongi)/180*3.1415926;		
			
			const double a = 6378245.00;		
			const double b = 6356863.50;		
			double sqre1 = (a*a-b*b)/(a*a);		

			double sinb = Math.Sin(rB);
			double cosb = Math.Cos(rB);
			double M = a*(1-sqre1)/(1-sqre1*sinb*sinb)/Math.Sqrt(1-sqre1*sinb*sinb);
			
			double N = a/Math.Sqrt(1-sqre1*sinb*sinb);
			double sqrita = N/M-1;
			
			
			double s = a*(1-sqre1)*(1.00505117739*rB-0.00506237764/2*Math.Sin(2*rB)+
				0.0000106245/4*Math.Sin(4*rB)-0.00000002081/6*Math.Sin(6*rB));

			double tanb = Math.Tan(rB);
			X = s+rL*rL*N/2*sinb*cosb+rL*rL*rL*rL*N/24*sinb*cosb*cosb*cosb*(5-tanb*tanb+9*sqrita*sqrita+4*sqrita);
			Y = rL*N*cosb+rL*rL*rL*N/6*cosb*cosb*cosb*(1-tanb*tanb+sqrita)+
				rL*rL*rL*rL*rL*N/120*cosb*cosb*cosb*cosb*cosb*(5-18*tanb*tanb+tanb*tanb*tanb*tanb);
			Y = Y+500000+nzonenum*1.0e+6;
		}

		#endregion

		#region accessCon
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public OleDbConnection GetAccessCon()
		{
			try
			{
				if( conn.State!=ConnectionState.Open )
				{
					conn.Open();
				}
				return conn;
			}
			catch(Exception e)
			{
				MessageBox.Show(e.Message ,"连接失败",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return null;
			}
		}
		#endregion
	}

    /// <summary>
    /// 
    /// </summary>
	public struct LayerInfo
	{
		public string dtName;
		public string dtFile;
		public double dtScale;
		public long dtSize;
		public long dtColor;
		public long dtSymbol;

		public long bzScale;
		public long bzSize;
		public long bzColor;

		public string typeName;
		public MapObjects2.MapLayer	layer;
	}

    /// <summary>
    /// 
    /// </summary>
	public struct PointInfo
	{
		public string s_Name;
		public string s_East;
		public string s_West;
		public string s_No;
	}

    /// <summary>
    /// 
    /// </summary>
	public class MPoint
	{
		public double x;
		public double y;

		public MPoint()
		{
		}
	}

    /// <summary>
    /// 存储stationFile.xml中的站点信息, 包括 name east west no 
    /// </summary>
	public class StationInfo
	{
		private PointInfo[] sInfo=null; 

        /// <summary>
        /// 
        /// </summary>
		public StationInfo()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
			XmlDocument xDoc=new XmlDocument();
			xDoc.Load("StationFile.xml");
			XmlNode xNode=xDoc.DocumentElement.SelectSingleNode("./table");
			sInfo=new PointInfo[xNode.ChildNodes.Count];
			for(int i=0;i<xNode.ChildNodes.Count;i++)
			{
				sInfo[i].s_Name=xNode.ChildNodes[i].InnerText.Trim();
				sInfo[i].s_East=xNode.ChildNodes[i].Attributes.GetNamedItem("east").Value.ToString().Trim();
				sInfo[i].s_West=xNode.ChildNodes[i].Attributes.GetNamedItem("west").Value.ToString().Trim();
				sInfo[i].s_No=xNode.ChildNodes[i].Attributes.GetNamedItem("no").Value.ToString().Trim();
			}
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public PointInfo[] GetPointInfo()
		{
			return sInfo;
		}
	}
}
