/*
 * ccsh.dpDown.js
 * 下来HTML控件
 * @author        周瑞
 * @version       1.0.0
 * @requires      jQuery 1.9+
 * @license 
 * Copyright 2013 中华商务联合印刷有限公司
 * 使用方法很简单
 * 1、<div id ="dropdown"></div>
 * 2、$("#dropdown").dpDown({
 *              displayField：  //需显示的字段名
 *              keyField： //对应的value字段
 *              selectedValue ://选中的信息
 *              multiSelect ://是否多选 如false
 *              cols 列名[{
 *                         field: 'name'
 *                     }]
 *              data 数据对象
 *              onSelect :改变选择项触发的事件
 *              selected 选中的key
 *              selecte:设定选择项目传入key
 *            loader: {  直接从API加载数据
 *			        url: '/javascript/grid/data.php',
 *			            params: { pageSize: 10 },
 *			            autoLoad: true
 *		            }
 *          });
 * API 文档
 */
; if (typeof Object.create !== 'function') {
    Object.create = function (obj) {
        function F() { }
        F.prototype = obj;
        return new F();
    };
}
(function ($) {
    $.fn.dpDown = function (options) {
        var self = this;
        var $self = $(this);
        self.opts = options = $.extend({
            comma: ',',
            name: self.prop('name')
        }, options);
        self.selected = options.selected || [];
        var cols = options.cols;
        var cw = options.width, ch = options.height;
        $self.addClass("dropdown input-group");
        var tf = $('<input type="text" class="form-control" />');//输入框
        
        tf.prop({
            name: options.name,
            maxLength: options.maxLen
        });

        var btn = $('<div class="input-group-btn"><button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown">  <span class="caret"></span></button></div>');//右面按钮
        var ul = $('<ul class="dropdown-menu pull-right" role="menu"></ul>');//下拉区域
        btn.append(ul);
        $self.append(tf).append(btn);//拼接成下拉按钮
        
        if (options.readOnly) {
            tf.attr('readonly', 'readonly');
            tf.click(function () {
                btn.click();
                return false;
            });
        }

        tf.change(function () {
            if (!tf.val())
                self.select([]);
        }).blur(self.close);
        //选择数据
        self.select = function (dd) {
            if (!$.isArray(dd))
                dd = dd ? (dd + '').split(options.comma) : [];
            if (!options.multiSelect && dd.length > 1)
                dd = [dd[0]];
            var dv = [], sd = [], sk = [], df = options.displayField;
            if (options.boxEl) {
                $.each(dd, function (n, d) {
                    dv.push(d[df]);
                });
            } else {
                dd = $.map(dd, function (c) {
                    return c + '';
                });
                ul.find('li').each(function (n, c) {
                    var a = $(c), k = a.attr('name');
                    var d = self.data[k];
                    if (dd.length > 0) {
                        if ($.inArray(k, dd) > -1) {
                            dv.push(d[df]);
                            sk.push(k);
                            sd.push(d);
                         
                        }
                    }
                });
            }
            self.selected = sk;
            dv = self.displayValue = dv.join(options.comma);
            tf.val(dv);
            if (options.onSelect)//执行外部的选择事件
                options.onSelect(self, sk, sd);
            return self;
        }
        //加载数据
        self.loadData = function (dd) {
            ul.empty();
            if (!dd || !dd.length)
                return;
            self.data = {};
            var mc = cols && cols.length;
            $.each(dd, function (i, r) {
                var kf = options.keyField, df = options.displayField;
                var k = kf ? r[kf] : 'r' + i;
                var li = $('<li role="presentation"></li>').attr('name', k);
                //li.on('click', self, options.onSelectChanged);
                var a = $('<a href="javascript:void(0)""></a>')
                self.data[k] = r;
                if (mc) {
                    $.each(cols, function (n, c) {
                        if (c.hidden)
                            return;
                        var b = $('<span style="display: inline-block" role="menuitem" tabindex="-1" ></span>');
                        b.width(c.width);
                        b.append(c.render ? c.render(r, b, self) : r[c.field]);
                        a.append(b);
                    });
                } else {
                    var b = $('<span style="display: inline-block" role="menuitem" tabindex="-1" ></span>');
                    b.append(r[options.displayField]).width(ul.width());
                    a.append(b);
                }
                li.append(a);
                li.mousedown(function () {
                    var sl = self.selected || [];
                    var i = $.inArray(k, sl), b = i > -1;
                    if (options.multiSelect) {
                        b ? sl.splice(i, 1) : sl.push(k);
                        self.select(sl);
                        return false;
                    } else {
                        self.select(b ? [] : [k]);
                    }

                });
                ul.append(li);
            });
            if (options.onLoad)
                options.onLoad(self, dd);
        }
        var ldr = options.loader;
        if (options.data) {
            self.loadData(options.data);
        } else if (ldr && ldr.url) {
            getWebJson(ldr.url, options.param, function (data) {
                self.loadData(data);
            });
        }
        return self;
    }
})(jQuery);