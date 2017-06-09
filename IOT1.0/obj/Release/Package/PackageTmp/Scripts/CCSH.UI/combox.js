/*
 * combox
 * api:
 *          $().combox({
 *             url: null,//url
 *             method: "GET",
 *             dataType: "json",
 *             param: {},//参数
 *             display: null,//显示字段
 *             value: null,//值
 *             data: [],//显示数据
 *             readOnly: true,//只读
 *             parent: null //{ name: null, value: null }//父级
 *        });
 * demo:
 *          $("#test").combox({ display: "name", value: "value", data: [{ name: "1", value: "1" }, { name: "2", value: "2" }] });
 *          var dept = $(".combox").combox({ display: "Dept_Name", value: "Dept_ID", url: "api/HR/GetDepartment" });
 *          $(".combox-sub").combox({ display: "Dept_Name", value: "Dept_ID", parent: { name: "parent", value: dept }, url: "api/HR/   *          GetChildDepartment" });
 *          $(".combox").onComboxSelect(function (t, item) {
 *              console.debug(item);
 *          });
 */
if (typeof Object.create !== 'function') {
    Object.create = function (obj) {
        function F() { }
        F.prototype = obj;
        return new F();
    };
}
(function ($) {
    $.addCombox = function (t, p) {
        if (t.combox) return false;//return if already exist
        p = $.extend({
            url: null,//url
            method: "GET",
            dataType: "json",
            param: {},//参数
            display: null,//显示字段
            value: null,//值
            data: [],//显示数据
            readOnly: true,//只读
            parent: null //{ name: null, value: null }//父级
        }, p);

        //create combox class
        var g = {
            selectedItem: null,//选择的数据项
            addData: function (data) {//加载数据
                self = this;
                if (!data || !data.length)
                    return;
                $(data).each(function (index, item) {
                    var value = item[p.display];
                    var li = $('<li role="presentation"></li>');
                    var a = $('<a href="javascript:void(0)"></a>');
                    var b = $('<span style="display: inline-block" role="menuitem" tabindex="-1" ></span>');
                    b.text(value);
                    a.append(b);
                    a.click(function () {//选择项
                        var value = item[p.display];
                        g.text.val(value);
                        $(t).attr("text", value).attr("value", item[p.value]);
                        self.selectedItem = item;
                        $(t).trigger("onComboxSelect");
                    });
                    li.append(a);
                    ul.append(li);
                });
            },
            loadData: function (param) {//从服务器端加载数据
                $.ajax({
                    type: p.method,
                    url: p.url,
                    data: param,
                    dataType: p.dataType,
                    success: function (data) {
                        g.addData(data);
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        try {
                            if (p.onError) p.onError(XMLHttpRequest, textStatus, errorThrown);
                        } catch (e) { }
                    }
                });
            },
            init: function () {
                ul.empty();
                g.text.val("");
                if (p.data.length) {
                    g.addData(p.data);
                } else {
                    if (p.url) {
                        g.loadData(p.param);
                    }
                }
            },
            load: function () {
                if (p.parent) {
                    $(p.parent.value).on("onComboxSelect", function () {
                        p.param[p.parent.name] = $(this).attr("value");
                        g.init();
                    });
                } else {
                    g.init();
                }
            },
            onselect: function (t, item) { }
        }

        $(t).show()//show if hidden
            .addClass("dropdown input-group"); //add class

        g.text = $('<input type="text" class="form-control" />');//输入框
        g.text.prop({
            name: p.name,
            maxLength: p.maxLen
        });
        var btn = $('<div class="input-group-btn"><button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">  <span class="caret"></span></button></div>');//右面按钮
        var ul = $('<ul class="dropdown-menu pull-right" role="menu"></ul>');//下拉区域
        btn.append(ul);
        $(t).append(g.text).append(btn);//拼接成下拉按钮



        g.load();
        t.p = p;
        t.dplist = g;
        return t;
    };
    var docloaded = false;
    $(document).ready(function () {
        docloaded = true;
    });
    $.fn.combox = function (p) {//create combox control
        return this.each(function () {
            if (!docloaded) {
                $(this).hide();
                var t = this;
                $(document).ready(function () {
                    $.addCombox(t, p);
                });
            } else {
                $.addCombox(this, p);
            }
        });
    };
    $.fn.onComboxSelect = function (handler) {
        return this.each(function () {
            if (this.dplist) {
                $(this).on("onComboxSelect", function () {
                    handler(this, this.dplist.selectedItem);
                });
            }
        });
    }
    $.fn.comboxReload = function (p) { // function to reload combox
        return this.each(function () {
            if (this.dplist) {
                $.extend(this.p, p);
                this.dplist.load();
            }
        });
    };
})(jQuery);