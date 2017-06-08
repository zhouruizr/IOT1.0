using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IOT1._0.Controllers
{
    public class UpLoadController : Controller
    {
        //
        // GET: /UpLoad/

       [HttpPost]
      public string Upload(FormCollection form)
      {
            if (Request.Files.Count == 0)
            {
　　　　　　    //Request.Files.Count 文件数为0上传不成功
　　　　　　    return  "0";　
　　　　    　}

            var file = Request.Files[0];
            if (file.ContentLength == 0)
            {
               //文件大小大（以字节为单位）为0时，做一些操作
                return "-1";　
　　　　      }
　　　　      else
　　　　      {
　　　　　　     //文件大小不为0
　　　　　     　HttpPostedFileBase upfile = Request.Files[0];
　　　　　　    //保存成自己的文件全路径,newfile就是你上传后保存的文件,
　　　　　　    //服务器上的UpLoadFile文件夹必须有读写权限　　　　　　
                //upfile.SaveAs(Server.MapPath(@"\App_Data"));
　　　　      }

　　　　      //newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + ".sl";
            Response.Write("<script type='text/javascript'>alert('第一项上传成功!');location.href='http://localhost:40315/Login.html';</script>");
            return "";
       }

       [HttpPost]
       public string Upload2(FormCollection form)
       {
           if (Request.Files.Count == 0)
           {
               //Request.Files.Count 文件数为0上传不成功
               return "0";
           }

           var file = Request.Files[0];
           if (file.ContentLength == 0)
           {
               //文件大小大（以字节为单位）为0时，做一些操作
               return "-1";
           }
           else
           {
               //文件大小不为0
               HttpPostedFileBase upfile = Request.Files[0];
               //保存成自己的文件全路径,newfile就是你上传后保存的文件,
               //服务器上的UpLoadFile文件夹必须有读写权限　　　　　　
               //upfile.SaveAs(Server.MapPath(@"\App_Data"));
           }

           //newFile = DateTime.Now.ToString("yyyyMMddHHmmss") + ".sl";
           Response.Write("<script type='text/javascript'>alert('第二项上传成功!');location.href='http://localhost:40315/Login.html';</script>");
           return "";
       }

       [HttpPost]
       public ActionResult Up(HttpPostedFileBase Filedata)
       {
           // 如果没有上传文件
           if (Filedata == null ||
               string.IsNullOrEmpty(Filedata.FileName) ||
               Filedata.ContentLength == 0)
           {
               return this.HttpNotFound();
           }

           // 保存到 ~/photos 文件夹中，名称不变
           string filename = System.IO.Path.GetFileName(Filedata.FileName);
           //string virtualPath =
           //    string.Format("~/photos/{0}", filename);
           //// 文件系统不能使用虚拟路径
           //string path = this.Server.MapPath(virtualPath);

           //Filedata.SaveAs(path);
           return this.Json(new { result = "上传成功", filename = filename,id = "1111" });
       }
    }
}
