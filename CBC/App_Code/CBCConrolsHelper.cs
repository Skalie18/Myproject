using System.Web.UI;
using System.Web.UI.Design.WebControls;
/// <summary>
/// Summary description for CBCConrolsHelper
/// </summary>
public static class CBCConrolsHelper
{
    public static void CreateRow4Columns(HtmlTextWriter writer, Control left, Control right, string leftLabel, string rightLabel)
    {
        /*
         * START ROW 1
         */

        writer.RenderBeginTag(HtmlTextWriterTag.Tr);

        //1
        writer.RenderBeginTag(HtmlTextWriterTag.Td);
        writer.Write(leftLabel);
        writer.RenderEndTag();

        //2
        writer.RenderBeginTag(HtmlTextWriterTag.Td);
        left.RenderControl(writer);
        writer.RenderEndTag();

        //3
        writer.RenderBeginTag(HtmlTextWriterTag.Td);
        writer.Write(rightLabel);
        writer.RenderEndTag();

        //4
        writer.RenderBeginTag(HtmlTextWriterTag.Td);
        right.RenderControl(writer);
        writer.RenderEndTag();


        writer.RenderEndTag();

        /*
         *END ROW 1
         */
    }
    public static void CreateRow2Columns(HtmlTextWriter writer, Control left, string leftLabel)
    {
        /*
         * START ROW 1
         */
      
        writer.RenderBeginTag(HtmlTextWriterTag.Tr);

        //1        
        writer.RenderBeginTag(HtmlTextWriterTag.Td);
        writer.Write(leftLabel);
        writer.RenderEndTag();

        //2

        writer.AddAttribute(HtmlTextWriterAttribute.Colspan, "3");
        writer.RenderBeginTag(HtmlTextWriterTag.Td);
         
        left.RenderControl(writer);
        writer.RenderEndTag();

        //3
      
        //writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //writer.Write(rightLabel);
        //writer.RenderEndTag();

        //4
        //writer.RenderBeginTag(HtmlTextWriterTag.Td);
        //right.RenderControl(writer);
        //writer.RenderEndTag();


        writer.RenderEndTag();

        /*
         *END ROW 1
         */
    }

}

public class TestDesigner : CompositeControlDesigner
{
    public override bool AllowResize
    {
        get { return false; }
    }
}

public class CBCControlDesigner : CompositeControlDesigner
{
    public override bool AllowResize
    {
        get { return false; }
    }
}

public class CBCAddressDesigner : CompositeControlDesigner
{
    public override bool AllowResize
    {
        get { return false; }
    }
}