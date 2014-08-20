using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Web;
using System.Resources;
using System.IO;
using Newtonsoft.Json;

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
        const string GlobalResourcesRelPath = "\\App_GlobalResources\\";
        const string FileExtensionSeparator = ".";
        const string ResxFileExtension = "resx";

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (context.Request.QueryString.AllKeys.Contains(ResourceFileNameQueryKey) && context.Request.QueryString.AllKeys.Contains(LanguageQueryKey))
                {
                    var filePath = context.Request.PhysicalApplicationPath + GlobalResourcesRelPath + context.Request.QueryString[ResourceFileNameQueryKey] + FileExtensionSeparator + context.Request.QueryString[LanguageQueryKey] + FileExtensionSeparator + ResxFileExtension;
                    if (File.Exists(filePath))
                        using (ResourceReader reader = new ResourceReader(filePath))
                        {
                            var tempDictionary = new Dictionary<string, string>();
                            var enumarator = reader.GetEnumerator();
                            while(enumarator.MoveNext())
                            {
                                tempDictionary.Add(enumarator.Key as string, enumarator.Value as string);
                            }
                            context.Response.ContentType = JsonContentType;
                            context.Response.Write(JsonConvert.SerializeObject(tempDictionary));
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
            catch (Exception)
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