using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web;
using System.Resources;
using System.IO;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace JSASPNetWebFormsLocalizeWeb
{
    /// <summary>
    /// Summary description for ScriptResx
    /// </summary>
    public class ScriptResx : IHttpHandler
    {
        const string ResourceFileNameQueryKey = "name";
        const string LanguageQueryKey = "culture";
        const string JsonContentType = "application/json";
        const string GlobalResourcesRelPath = "App_GlobalResources\\";
        const string FileExtensionSeparator = ".";
        const string ResxFileExtension = "resx";
        const string ResxKeyAttribute = "name";
        const string ResxValueElement = "value";
        const string ResxElement = "data";

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request.QueryString.AllKeys.Contains(ResourceFileNameQueryKey) && context.Request.QueryString.AllKeys.Contains(LanguageQueryKey))
                {
                    var filePath = context.Request.PhysicalApplicationPath + GlobalResourcesRelPath + context.Request.QueryString[ResourceFileNameQueryKey] + FileExtensionSeparator + context.Request.QueryString[LanguageQueryKey] + FileExtensionSeparator + ResxFileExtension;
                    if (File.Exists(filePath))//you can improve the behavior here if you want to load a default language in case you don't have that language
                    {
                        var rootNode = XElement.Load(filePath);
                        var tempDictionary = rootNode.Descendants(ResxElement)
                           .ToDictionary(x => x.Attribute(ResxKeyAttribute).Value, x => x.Descendants(ResxValueElement).First().Value);
                        context.Response.ContentType = JsonContentType;
                        context.Response.Write(JsonConvert.SerializeObject(tempDictionary));//here you can handle caching mecanisms if you want
                    }
                    else
                        throw new FileNotFoundException();
                }
                else
                    throw new FileNotFoundException();
            }
            catch (FileNotFoundException)
            {
                context.Response.StatusCode = 404;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}