namespace Sitecore.Support.ExperienceEditor.Speak.Ribbon.Requests.SaveItem
{
  using Sitecore.ExperienceEditor.Speak.Server.Responses;
  using System.Web;
  using System.Linq;
  using System;
  using Sitecore.Data;

  public class CallServerSavePipeline : Sitecore.ExperienceEditor.Speak.Ribbon.Requests.SaveItem.CallServerSavePipeline
  {
    public override PipelineProcessorResponseValue ProcessRequest()
    {
      var args = this.RequestContext.GetSaveArgs();
      DecodeFieldValues();
      return base.ProcessRequest();
    }

    private void DecodeFieldValues()
    {
      var keys = RequestContext.FieldValues.Keys.ToArray();
      foreach (var key in keys)
      {
        if (!key.StartsWith("fld_", StringComparison.InvariantCulture) &&
            !key.StartsWith("flds_", StringComparison.InvariantCulture)) continue;
        var key2 = key;
        var num = key2.IndexOf('$');
        if (num >= 0)
        {
          key2 = StringUtil.Left(key2, num);
        }
        var array3 = key2.Split('_');
        var iD = ShortID.DecodeID(array3[1]);
        var iD2 = ShortID.DecodeID(array3[2]);
        var item = RequestContext.Item.Database.GetItem(iD);
        if (item == null) continue;
        var field = item.Fields[iD2];
        var typeKey = field.TypeKey;
        if (typeKey != null && typeKey.Equals("single-line text", StringComparison.InvariantCultureIgnoreCase))
        {
          RequestContext.FieldValues[key] = HttpUtility.HtmlDecode(RequestContext.FieldValues[key]);
        }
      }
    }
  }
}