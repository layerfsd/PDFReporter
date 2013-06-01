using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;

using AxiomCoders.PdfTemplateEditor.Forms;
using AxiomCoders.PdfTemplateEditor.Common;

namespace AxiomCoders.PdfTemplateEditor.EditorStuff
{
    [Serializable, System.Reflection.Obfuscation(Exclude = true)]
    public class AnchorEditor : UITypeEditor
    {
        AnchorProperty editorDialog;
        public static Anchor itemsAnchor;
        public AnchorEditor()
        {

        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            editorDialog = new AnchorProperty();

            if(DialogResult.OK == editorDialog.Show(itemsAnchor))
            {
                value = true;
                EditorController.Instance.ProjectSaved = false;
            }
            else
            {
                value = false;
            }

            //value = "Border properties...";
            EditorController.Instance.EditorViewer.RefreshView();
            return value;// base.EditValue(context, provider, value);
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }
    }



    public class Anchor
    {
        private bool topAnchor = true;
        private bool leftAnchor = true;
        private bool rightAnchor = false;
        private bool bottomAnchor = false;

        private float startLocX = 0.0f;
        private float startLocY = 0.0f;
        private float startWidth = 0.0f;
        private float startHeight = 0.0f;
        private float leftKoef = 0.0f;
        private float topKoef = 0.0f;

        private EditorItem owner = null;

        public Anchor(EditorItem ownerItem)
        {
            owner = ownerItem;
        }

        public bool TopAnchor
        {
            get { return topAnchor; }
            set { topAnchor = value; }
        }

        public bool LeftAnchor
        {
            get { return leftAnchor; }
            set { leftAnchor = value; }
        }

        public bool RightAnchor
        {
            get { return rightAnchor; }
            set { rightAnchor = value; }
        }

        public bool Bottomanchor
        {
            get { return bottomAnchor; }
            set { bottomAnchor = value; }
        }

        public string GetAnchor()
        {
            string retStr = "";

            if(TopAnchor) retStr = retStr + "Top; ";
            if(Bottomanchor) retStr = retStr + "Bottom; ";
            if(LeftAnchor) retStr = retStr + "Left; ";
            if(RightAnchor) retStr = retStr + "Right; ";

            return retStr;
        }

        public void UpdateOwner(float duLocX, float duLocY, float dWidth, float dHeight)
        {
            if(TopAnchor)
            {
                if(Bottomanchor)
                {
                    owner.HeightInPixels = startHeight + dHeight;
                }
            }else{
                owner.LocationInUnitsY = startLocY - duLocY;
            }

            if(LeftAnchor)
            {
                if(RightAnchor)
                {
                    owner.WidthInPixels = startWidth + dWidth;
                }
            }else{
                owner.LocationInUnitsX = startLocX - duLocX;
            }
            

            if(RightAnchor && !LeftAnchor)
            {
                if(duLocX == 0.0f)
                {
                    float tmpDeltaU = UnitsManager.Instance.ConvertUnit(dWidth, MeasureUnits.pixel, owner.MeasureUnit);
                    owner.LocationInUnitsX = startLocX + tmpDeltaU;
                }
            }

            if(Bottomanchor && !TopAnchor)
            {
                if(duLocY == 0.0f)
                {
                    float tmpDeltaU = UnitsManager.Instance.ConvertUnit(dHeight, MeasureUnits.pixel, owner.MeasureUnit);
                    owner.LocationInUnitsY = startLocY + tmpDeltaU;
                }
            }

            if((TopAnchor || Bottomanchor) && !LeftAnchor && !RightAnchor)
            {
                owner.LocationInUnitsX = UnitsManager.Instance.ConvertUnit((owner.Parent.WidthInPixels * leftKoef), MeasureUnits.pixel, owner.MeasureUnit);
            }

            if((LeftAnchor || RightAnchor) && !TopAnchor && !Bottomanchor)
            {
                owner.LocationInUnitsY = UnitsManager.Instance.ConvertUnit((owner.Parent.HeightInPixels * topKoef), MeasureUnits.pixel, owner.MeasureUnit);
            }
        }

        /// <summary>
        /// Lock anchoring 
        /// </summary>
        public void LockPositions()
        {
            startLocX = owner.LocationInUnitsX;
            startLocY = owner.LocationInUnitsY;
            startWidth = owner.WidthInPixels;
            startHeight = owner.HeightInPixels;

            leftKoef = owner.LocationInPixelsX / owner.Parent.WidthInPixels;
            topKoef = owner.LocationInPixelsY / owner.Parent.HeightInPixels;
        }
    }
}
