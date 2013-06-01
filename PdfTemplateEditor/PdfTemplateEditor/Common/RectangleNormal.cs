using System;
using System.Collections.Generic;
using System.Text;

namespace AxiomCoders.PdfTemplateEditor.Common
{
    public class RectangleNormal
    {
        public float top;
        public float left;
        public float bottom;
        public float right;

        public RectangleNormal()
        {
        }


        public bool Intersect(RectangleNormal rect)
        {
			return !((this.left - rect.right >= -0.001f)
			|| (rect.left - this.right >= -0.001f)
			|| (this.top - rect.bottom >= -0.001f)
			|| (rect.top - this.bottom >= -0.001f));

            /*return !(this.left >= rect.right
                || rect.left >= this.right
                || this.top >= rect.bottom
                || rect.top >= this.bottom);*/
        }


        public bool Contains(RectangleNormal rect)
        {
			return ((rect.left - this.left >= -0.001f)
			&& (this.right - rect.right >= -0.001f)
			&& (rect.top - this.top >= -0.001f)
			&& (this.bottom - rect.bottom >= -0.001f));

            /*return (rect.left >= this.left
                && rect.right <= this.right
                && rect.top >= this.top
                && rect.bottom <= this.bottom);*/
        }



        public void MoveTo(float locX, float locY)
        {
            float w, h;
            w = this.right - this.left;
            h = this.bottom - this.top;

            this.left = locX;
            this.top = locY;

            this.right = (this.left + w);
            this.bottom = (this.top + h);		
        }
    }
}
