var CurrentAccount = CurrentAccount || null;//登陆信息
var TreeMenuId = -1;
var SiteRoot = "http://localhost:40315/"; //CCSH.AccountRights网站根目录，用于WEBAPI，本地测试
//var SiteRoot = "http://192.168.1.248/iot/"; //CCSH.AccountRights网站根目录，用于WEBAPI，发布到服务器用地址,正式库

////从URL获取参数信息
//var loading;
//$.getScript("scripts/CCSH.UI/jquery.Loading.js", function () {
//     loading = $.loading();
//});//动态加载JS
//var loading = $.loading();
function getParam(paramName) {
    paramValue = ""; 
    isFound = false;
    if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
        arrSource = decodeURI(this.location.search).substring(1, this.location.search.length).split("&");//decodeURI解码和菜单加密方式一样
        i = 0;
        while (i < arrSource.length && !isFound) {
            if (arrSource[i].indexOf("=") > 0) {
                if (arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase()) {
                    paramValue = arrSource[i].split("=")[1];
                    isFound = true;
                }
            }
            i++;
        }
    }
    return paramValue;//decodeURI(paramValue)
}

$(function () {

});
var CCSHmobi = {
    author: 'CCSH',
    version: '1.0',
    website: SiteRoot + 'api/'  //CCH.ERP/
}
//本地存储
var DBoperate = {
    set: function (name, value) {
        localStorage.setItem(name, value)
    },
    get: function (name) {
        return localStorage.getItem(name)
    }
}

/*
 * 异步调用json
 * 可以使用可变参数{}或cmd, data, succfn, errfn
 */
function getWebJson(cmd, data, succfn, errfn) {
    var param = { type: "GET", dataType: "json", async: true };
    if (arguments.length == 1 && typeof arguments[0] == "object") {
        param = $.extend(param, arguments[0]);
        param.url = CCSHmobi.website + param.url;
    } else {
        param = $.extend(param, { url: CCSHmobi.website + arguments[0], data: arguments[1], success: arguments[2], error: arguments[3] || null });
    }
    $.ajax(param);
    //$.ajax({
    //    url: CCSHmobi.website + cmd,
    //    type: 'GET',
    //    dataType: 'json',
    //    async:true,
    //    data: data,
    //    success: function (response) {
    //        if (succfn) {
    //            succfn(response);
    //        }
    //    },
    //    error: function (response) {
    //        if (errfn) {
    //            errfn(response);
    //        }
    //    }
    //});
}
function postWebJson(cmd, data, fn, errorfn) {

    if (typeof data == 'object') {
        data = JSON.stringify(data);
    }
    $.ajax(
		{
		    type: "POST",
		    url: CCSHmobi.website + cmd,
		    contentType: "application/json",
		    data: data,
		    success: fn,
		    error: function (req, status, error) {
		        if (errorfn != null)
		            errorfn();
		        alert(req.responseText);
		        //console.debug(req.responseText)
		        //console.debug(status)
		        //console.debug(error)
		        //window.location.href = "SpecificationList.html";
		    },
		    complete: function (XMLHttpRequest, textStatus) {

		    }
		}
      )
};


function errorHandle(XMLHttpRequest, textStatus, errorThrown) {
    alert(textStatus + ": " + XMLHttpRequest.responseText);
}

//获取当月的第一天
function GetFirstDayOrMonth() {
    var myDate = new Date();
    var year = myDate.getFullYear();
    var month = myDate.getMonth() - 1;
    if (month < 10) {
        month = "0" + month;
    }
    var firstDay = month + "/" + "01/" + year;
    return year + "-" + month + "-" + "01";
}
//获取当月的最后一天
function getLastDayOfMonth() {
    var myDate = new Date();
    var year = myDate.getFullYear();
    var new_year = year;    //取当前的年份         
    var new_month = myDate.getMonth() + 1;;//取下一个月的第一天，方便计算（最后一天不固定）         
    if (new_month > 12)            //如果当前大于12月，则年份转到下一年         
    {
        new_month -= 12;        //月份减         
        new_year++;            //年份增         
    }
    var new_date = new Date(new_year, new_month, 1);                //取当年当月中的第一天         
    var date_count = (new Date(new_date.getTime() - 1000 * 60 * 60 * 24)).getDate();//获取当月的天数       
    var last_date = new Date(new_date.getTime() - 1000 * 60 * 60 * 24);//获得当月最后一天的日期
    return year + "-" + new_month + "-" + date_count;
}


//绑定数据通用方法
function bingData(cmd, para, v_moduleArray, successFn) {
    //if (v_moduleArray) {
    //    v_moduleArray.removeAll();//首先清空
    //}
    //postWebJson(cmd, para, function (data) {
    //    if (v_moduleArray) {
    //        for (var i = 0, max = data.length; i < max; i++) {
    //            v_moduleArray.push(data[i]);
    //        }
    //    }
    //    if (successFn) {
    //        successFn();
    //    }
    //})
    if (v_moduleArray && cmd) {
        v_moduleArray.removeAll();

        postWebJson(cmd, para, function (responseJS) {
            v_moduleArray(responseJS.slice());

            if (successFn) {
                successFn();
            }
        });
    }
};

// 对Date的扩展，将 Date 转化为指定格式的String   
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
// 例子：   
// (new Date()).Format("yyyy-MM-dd HH:mm:ss.S") ==> 2006-07-02 08:09:04.423   
// (new Date()).Format("yyyy-M-d H:m:s.S")      ==> 2006-7-2 8:9:4.18   
Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "H+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), // 季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
}

/* 
* 获得时间差,时间格式为 年-月-日 小时:分钟:秒 或者 年/月/日 小时：分钟：秒 
* 其中，年月日为全格式，例如 ： 2010-10-12 01:00:00 
* 返回精度为：秒，分，小时，天
var result = GetDateDiff("2010-02-26 16:00:00", "2011-07-02 21:48:40", "day");

使用的方法很简单，比如计算天数可以这样：

GetDateDiff("2010-02-26 16:00:00", "2011-07-02 21:48:40", "day");

计算秒数则可以这样：

GetDateDiff("2010-02-26 16:00:00", "2011-07-02 21:48:40", "second");
*/
function GetDateDiff(startTime, endTime, diffType) {
    //将xxxx-xx-xx的时间格式，转换为 xxxx/xx/xx的格式 
    startTime = startTime.replace(/\-/g, "/");
    endTime = endTime.replace(/\-/g, "/");
    //将计算间隔类性字符转换为小写
    diffType = diffType.toLowerCase();
    var sTime = new Date(startTime);      //开始时间
    var eTime = new Date(endTime);  //结束时间
    //作为除数的数字
    var divNum = 1;
    switch (diffType) {
        case "second":
            divNum = 1000;
            break;
        case "minute":
            divNum = 1000 * 60;
            break;
        case "hour":
            divNum = 1000 * 3600;
            break;
        case "day":
            divNum = 1000 * 3600 * 24;
            break;
        default:
            break;
    }
    return parseInt((eTime.getTime() - sTime.getTime()) / parseInt(divNum));
}



function ConvertToDate(objStr) {
    if (objStr != null && objStr != "") {
        var str = objStr.replace('T', ' ').replace(/-/g, '/');
        var date = new Date(str);
        return date;
    } else {
        return null;
    }
}

//清除数据cleanData("container");
function cleanData(container) {
    var _container = $("#" + container);
    $("input:text", _container).val("").trigger("change");//清除文本框
    $("textarea", _container).val("").trigger("chagne");//清除文本域
    $("input:password", _container).val("").trigger("change");//清除密码框
    $("input:checkbox:checked", _container).attr("checked", false).trigger("change");//清除复选框
    $("input:radio:checked", _container).attr("checked", false).trigger("change");//清除点选框
    $("select", _container).each(function () {//清除下拉框
        this.selectedIndex = 0;
        $(this).trigger("change");
    });
    $("div[type=ComboBox]").each(function () {//清除combobox控件
        this.clean();
    });
    $("table[type=grid]").each(function () {//清除flexigrid控件
        $(this).removeData();
    });
}

//清除数据cleanData("container");小模型，不包含grid
function cleanDataSmall(container) {
    var _container = $("#" + container);
    $("input:text", _container).val("").trigger("change");//清除文本框
    $("textarea", _container).val("").trigger("chagne");//清除文本域
    $("input:password", _container).val("").trigger("change");//清除密码框
    $("input:checkbox:checked", _container).attr("checked", false).trigger("change");//清除复选框
    $("input:radio:checked", _container).attr("checked", false).trigger("change");//清除点选框
    $("select", _container).each(function () {//清除下拉框
        this.selectedIndex = 0;
        $(this).trigger("change");
    });
    $("div[type=ComboBox]").each(function () {//清除combobox控件
        this.clean();
    });
}


//信息提示框封装开始
var msgset = function setbootbox() {
    bootbox.setDefaults({ title: '提示:' });
}

var msg = {
    ok: function () {
        msgset();
        bootbox.alert(' 保存成功！');
    },
    err: function () {
        msgset();
        bootbox.alert(' 未保存任何数据！');
    },
    info: function (d) {
        msgset();
        bootbox.alert(d.toString());
    },
}
//信息提示框封装结束