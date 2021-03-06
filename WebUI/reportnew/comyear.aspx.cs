﻿
    using DBUtility;
    using DevExpress.XtraCharts;
    using DevExpress.XtraCharts.Web;
    using System;
    using System.Data;
    using System.Drawing;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public partial class  comyear : Page
    {
        private BaseClass bc = new BaseClass();
        private ViewType types = ViewType.Bar;
        public string year = DateTime.Now.ToString("yyyy-MM-dd");

        private void DrawPie()
        {
            string title = DateTime.Parse(this.year).AddYears(-1).ToString("yyyy") + "年与" + DateTime.Parse(this.year).ToString("yyyy") + "年的会员消费金额统计分析图";
            ChartServices.SetChartTitle(this.WebChartControl1, true, title, StringAlignment.Center, ChartTitleDockStyle.Top, true, new Font("宋体", 12f, FontStyle.Bold), Color.Red, 10);
            if (this.types == ViewType.Bar)
            {
                ChartServices.DrawChart(this.WebChartControl1, "会员卡", this.types, this.load(), "类型", "会员卡");
                ChartServices.DrawChart(this.WebChartControl1, "临时卡", this.types, this.load(), "类型", "临时卡");
                ChartServices.DrawChart(this.WebChartControl1, "散客卡", this.types, this.load(), "类型", "散客卡");
            }
            else
            {
                ChartServices.DrawChart(this.WebChartControl1, "上年", this.types, this.loadz(), "CardType", "上年");
                ChartServices.DrawChart(this.WebChartControl1, "本年", this.types, this.loadz(), "CardType", "本年");
            }
        }

        public DataTable load()
        {
            string sql = "select CardType,sum(case datediff(year,Addtime,'" + this.year + "') when 0 then CONVERT(decimal(18, 2),[money]) else 0.00 end) as '本年',sum(case DATEDIFF(year,Addtime,'" + this.year + "') when 1 then CONVERT(decimal(18, 2),[money]) else 0.00 end) as '上年' from Consumption_Back_select group by CardType ";
            DataTable dt = this.bc.ReadTable(sql);
            DataTable dtz = new DataTable();
            dtz.Columns.Add("类型", typeof(string));
            dtz.Columns.Add("会员卡", typeof(decimal));
            dtz.Columns.Add("临时卡", typeof(decimal));
            dtz.Columns.Add("散客卡", typeof(decimal));
            if (dt.Rows.Count == 0)
            {
                dtz.Rows.Add(new object[] { dt.Columns[2].ColumnName, "0", "0", "0" });
                dtz.Rows.Add(new object[] { dt.Columns[1].ColumnName, "0", "0", "0" });
                return dtz;
            }
            if (dt.Rows.Count == 1)
            {
                dtz.Rows.Add(new object[] { dt.Columns[2].ColumnName, dt.Rows[0][2].ToString(), "0", "0" });
                dtz.Rows.Add(new object[] { dt.Columns[1].ColumnName, dt.Rows[0][1].ToString(), "0", "0" });
                return dtz;
            }
            if (dt.Rows.Count == 2)
            {
                dtz.Rows.Add(new object[] { dt.Columns[2].ColumnName, dt.Rows[0][2].ToString(), dt.Rows[1][2].ToString(), "0" });
                dtz.Rows.Add(new object[] { dt.Columns[1].ColumnName, dt.Rows[0][1].ToString(), dt.Rows[1][1].ToString(), "0" });
                return dtz;
            }
            dtz.Rows.Add(new object[] { dt.Columns[2].ColumnName, dt.Rows[0][2].ToString(), dt.Rows[1][2].ToString(), dt.Rows[2][2].ToString() });
            dtz.Rows.Add(new object[] { dt.Columns[1].ColumnName, dt.Rows[0][1].ToString(), dt.Rows[1][1].ToString(), dt.Rows[2][1].ToString() });
            return dtz;
        }

        public DataTable loadz()
        {
            string sql = "select CardType,sum(case datediff(year,Addtime,'" + this.year + "') when 0 then CONVERT(decimal(18, 2),[money]) else 0.00 end) as '本年',sum(case DATEDIFF(year,Addtime,'" + this.year + "') when 1 then CONVERT(decimal(18, 2),[money]) else 0.00 end) as '上年' from Consumption_Back_select group by CardType ";
            return this.bc.ReadTable(sql);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                try
                {
                    this.year = DateTime.Parse(base.Request.QueryString["year"].ToString()).ToString("yyyy-MM-dd");
                    if (base.Server.UrlDecode(base.Request.QueryString["type"].ToString()) == "树状图")
                    {
                        this.types = ViewType.Bar;
                    }
                    else
                    {
                        this.types = ViewType.Pie;
                    }
                }
                catch
                {
                }
                this.DrawPie();
            }
        }
    }


