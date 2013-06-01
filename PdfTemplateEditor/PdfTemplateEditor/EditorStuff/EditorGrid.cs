using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

using AxiomCoders.PdfTemplateEditor.Common;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{

    public class GridLine
    {
        public GridLine()
        {
        }

        public float x1, x2;
        public float y1, y2;
    }


    public class EditorGrid
    {
        private MeasureUnits unit = MeasureUnits.inch;
        private bool snap;
        private Color lineColor = Color.DarkGray;
        private Pen majorLinePen;
        private Pen minorLinePen;
        private float lineWidth = 1.0F;
        private bool showGrid;
        private bool showMinorLines = true;        
        private float subdivisions = 4.0F;
        private bool linesMaked;
		private float gridInterval = -1;
        private System.Drawing.Drawing2D.DashStyle lineStyle = System.Drawing.Drawing2D.DashStyle.Solid;

        private List<GridLine> majorHorizontal = new List<GridLine>();
        private List<GridLine> majorVertical = new List<GridLine>();
        private List<GridLine> minorHorizontal = new List<GridLine>();
        private List<GridLine> minorVertical = new List<GridLine>();
        

        public EditorGrid()
        {
            majorLinePen = new Pen(AppOptions.Instance.GridColor, lineWidth);
            majorLinePen.DashStyle = lineStyle;
            minorLinePen = new Pen(AppOptions.Instance.GridColor, lineWidth);
            minorLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
        }


        public MeasureUnits Unit
        {
            get { return unit; }
            set 
            {
                if(unit != value)
                {
                    unit = value;
                    MakeAllLines();
                }
            }
        }



        public System.Drawing.Drawing2D.DashStyle LineStyle
        {
            get { return lineStyle; }
            set { lineStyle = value; majorLinePen.DashStyle = lineStyle; }
        }

		       

        public bool Snap
        {
            get { return snap; }
            set { snap = value; }
        }



        public Color LineColor
        {
            get { return lineColor; }
            set 
            {
                lineColor = value; 
                majorLinePen.Brush = new SolidBrush(lineColor);
                minorLinePen.Brush = new SolidBrush(lineColor);
            }
        } 

        public float LineWidth
        {
            get { return lineWidth; }
            set 
            { 
                lineWidth = value;
                majorLinePen.Width = lineWidth;
                minorLinePen.Width = lineWidth;
            }
        }



        private void MakeAllLines()
        {
            majorHorizontal.Clear();
            majorVertical.Clear();
            minorHorizontal.Clear();
            minorVertical.Clear();

            float endWidth = UnitsManager.Instance.ConvertUnit(EditorController.Instance.EditorProject.ReportPage.WidthInPixels, MeasureUnits.pixel, this.unit);
            float endHeighit = UnitsManager.Instance.ConvertUnit(EditorController.Instance.EditorProject.ReportPage.HeightInPixels, MeasureUnits.pixel, this.unit);
            int numOfHLines = (int)(endHeighit / this.gridInterval);
			int numOfVLines = (int)(endWidth / this.gridInterval);

            //Make major lines
            for(int i = 0; i <= numOfHLines; i++)
            {
                GridLine tmpLine = new GridLine();
                float result = 0.0F;

				result = (float)i * this.gridInterval;
                tmpLine.x1 = 0.0F;
                tmpLine.y1 = result;
                tmpLine.x2 = endWidth;
                tmpLine.y2 = result;

                majorHorizontal.Add(tmpLine);
            }

            for(int i = 0; i <= numOfVLines; i++)
            {
                GridLine tmpLine = new GridLine();
                float result = 0.0F;

				result = (float)i * this.gridInterval;
                tmpLine.x1 = result;
                tmpLine.y1 = 0.0F;
                tmpLine.x2 = result;
                tmpLine.y2 = endHeighit;
                

                majorVertical.Add(tmpLine);
            }
            

            //Make minor lines
            int counter = 0;
            for(int i = 0; i <= (numOfHLines + 1)* subdivisions; i++)
            {
                if(counter == 0)
                {
                    counter++;
                    continue;
                }
                else
                {
                    if(counter == (int)subdivisions)
                    {
                        counter = 1;
                        continue;
                    }
                    else
                    {
                        GridLine tmpLine = new GridLine();
                        float result = 0.0F;

						result = (float)i * this.gridInterval / subdivisions;
                        if(result < endHeighit)
                        {
                            tmpLine.x1 = 0.0F;
                            tmpLine.y1 = result;
                            tmpLine.x2 = endWidth;
                            tmpLine.y2 = result;

                            minorHorizontal.Add(tmpLine);
                        }
                        counter++;
                    }
                }
            }


            counter = 0;
            for(int i = 0; i <= (numOfVLines + 1) * subdivisions; i++)
            {
                if(counter == 0)
                {
                    counter++;
                    continue;
                }
                else
                {
                    if(counter == (int)subdivisions)
                    {
                        counter = 1;
                        continue;
                    }
                    else
                    {
                        GridLine tmpLine = new GridLine();
                        float result = 0.0F;

						result = (float)i * this.gridInterval / subdivisions;
                        if(result < endWidth)
                        {
                            tmpLine.x1 = result;
                            tmpLine.y1 = 0.0F;
                            tmpLine.x2 = result;
                            tmpLine.y2 = endHeighit;

                            minorVertical.Add(tmpLine);
                        }
                        counter++;
                    }
                }
            }
            linesMaked = true;
        }




        private void UpdateGridOptions()
        {
            if((gridInterval != AppOptions.Instance.GridInterval) || 
			   (this.subdivisions != AppOptions.Instance.GridSubdivisions))
            {
				gridInterval = AppOptions.Instance.GridInterval;
				subdivisions = AppOptions.Instance.GridSubdivisions;

                MakeAllLines();
            }

			majorLinePen.Color = AppOptions.Instance.GridColor;
			minorLinePen.Color = AppOptions.Instance.GridColor;

            switch (AppOptions.Instance.GridLineStyle)
            {
                case 0: LineStyle = System.Drawing.Drawing2D.DashStyle.Solid; break;
                case 1: LineStyle = System.Drawing.Drawing2D.DashStyle.Dash; break;
                case 2: LineStyle = System.Drawing.Drawing2D.DashStyle.Dot; break;
            }

            switch (AppOptions.Instance.GridUnit)
            {
                case 0: Unit = MeasureUnits.inch; break;
                case 1: Unit = MeasureUnits.cm; break;
                case 2: Unit = MeasureUnits.mm; break;
                case 3: Unit = MeasureUnits.pixel; break;
            }
        }




        public void Draw(Graphics gc)
        {
            if (!AppOptions.Instance.ShowGrid)
            {
                return;
            }

            UpdateGridOptions();

            DrawMinorLines(gc);
            DrawMajorLines(gc);            
        }


       

        private void DrawMajorLines(Graphics gc)
        {
            gc.Transform = EditorController.Instance.EditorProject.CurrentReportPage.LastDrawMatrix;

            foreach(GridLine tmpLine in majorHorizontal)
            {
				float zoom = (float)EditorController.Instance.EditorProject.CurrentReportPage.ZoomLevel / 100.0f;

				majorLinePen.Width = 1.0f / zoom;
                gc.DrawLine(majorLinePen, UnitsManager.Instance.ConvertUnit(tmpLine.x1, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.y1, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.x2, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.y2, this.Unit, MeasureUnits.pixel));
            }

            foreach(GridLine tmpLine in majorVertical)
            {
				float zoom = (float)EditorController.Instance.EditorProject.CurrentReportPage.ZoomLevel / 100.0f;

				majorLinePen.Width = 1.0f / zoom;

                gc.DrawLine(majorLinePen, UnitsManager.Instance.ConvertUnit(tmpLine.x1, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.y1, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.x2, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.y2, this.Unit, MeasureUnits.pixel));
            }
        }




        private void DrawMinorLines(Graphics gc)
        {
            if(!AppOptions.Instance.ShowGridMinorLines)
            {
                return;
            }

            gc.Transform = EditorController.Instance.EditorProject.ReportPage.LastDrawMatrix;

            foreach(GridLine tmpLine in minorHorizontal)
            {
				float zoom = (float)EditorController.Instance.EditorProject.CurrentReportPage.ZoomLevel / 100.0f;

				minorLinePen.Width = 1.0f / zoom;
                gc.DrawLine(minorLinePen, UnitsManager.Instance.ConvertUnit(tmpLine.x1, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.y1, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.x2, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.y2, this.Unit, MeasureUnits.pixel));
            }

            foreach(GridLine tmpLine in minorVertical)
            {
				float zoom = (float)EditorController.Instance.EditorProject.CurrentReportPage.ZoomLevel / 100.0f;

				minorLinePen.Width = 1.0f / zoom;

                gc.DrawLine(minorLinePen, UnitsManager.Instance.ConvertUnit(tmpLine.x1, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.y1, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.x2, this.Unit, MeasureUnits.pixel),
                                          UnitsManager.Instance.ConvertUnit(tmpLine.y2, this.Unit, MeasureUnits.pixel));
            }
        }
    }
}
