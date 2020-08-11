using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Models
{
    public class XYPair<Tx, Ty>
    {
        public Tx x { get; set; }
        public Ty y { get; set; }
        public string tooltip { get; set; }
    }

    public class DataPoint
    {
        public DataPoint()
        {
            data = new List<XYPair<object, object>>();
        }
        public List<XYPair<object, object>> data { get; set; }
    }

    public class XYRange
    {
        public double max_y { get; set; }
        public double min_y { get; set; }
        public object max_x { get; set; }
        public object min_x { get; set; }
    }

    public class Series
    {
        public Series()
        {
            data = new DataPoint();
        }
        public string name { get; set; }
        public EChartType type { get; set; }
        public DataPoint data { get; set; }
        public XYRange range { get; set; } = new XYRange();
    }

    /// <summary>
    /// 线图序列
    /// </summary>
    public class LineSeries : Series
    {
        //扩展特殊需求，比如 DashStyle，Step 等.
        public EChartDashStyle dash_style { get; set; } = EChartDashStyle.Solid;

        public EChartStep? step { get; set; } = EChartStep.left;

        /// <summary>
        /// 默认并列显示，normal表示叠加显示
        /// </summary>
        public string stacking { get; set; }
    }

    public enum EChartType
    {
        /// <summary>
        /// 直线
        /// </summary>
        line,
        /// <summary>
        /// 曲线
        /// </summary>
        spline,
        /// <summary>
        /// 散点
        /// </summary>
        scatter,
        /// <summary>
        /// 条状
        /// </summary>
        bar,
        /// <summary>
        /// 柱状
        /// </summary>
        column,
        /// <summary>
        /// 饼状
        /// </summary>
        pie,
        /// <summary>
        /// 区块
        /// </summary>
        area,
        /// <summary>
        /// 3D泡泡
        /// </summary>
        bubble,
        /// <summary>
        /// 热力图
        /// </summary>
        heatmap
    }

    public enum EChartStep
    {
        /// <summary>
        /// 步长类型，点在水平线的左边
        /// </summary>
        left,
        /// <summary>
        /// 步长类型，点在水平线的中间
        /// </summary>
        center,
        /// <summary>
        /// 步长类型，点在水平线的右边
        /// </summary>
        right
    }

    public enum EChartDashStyle
    {
        /// <summary>
        /// 实线
        /// </summary>
        Solid,
        /// <summary>
        /// 点线
        /// </summary>
        Dot,
        /// <summary>
        /// 虚线
        /// </summary>
        Dash
    }

    public class ChartSeries
    {
        public string title { get; set; }

        public string plot_label { get; set; }

        public object plot_value { get; set; }

        public List<LineSeries> line_series { get; set; } = new List<LineSeries>();
    }
}
