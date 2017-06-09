/**
 * CCSH.Toolbar.js
 * 根据权限生成按钮组，需要引用BootStrap的样式文件，中华商务专业插件之一。
 * @author        周瑞
 * @version       1.0.0
 * @requires      jQuery 1.9+
 * @license 
 * Copyright 2013 中华商务联合印刷有限公司
 * 使用方法很简单
 * 1、<div id ="toolbar"></div>
 * 2、$("#toolbar").toolbar({
 *              treeId: 18 //传入对应的treeId即可，会找到对应的ERBAPI返回按钮组
 *          });
 * API 文档
 * treeId ：菜单的ID号用于权限管理，可以返回按钮组
 * toolBarBtnClick: 点击按钮时执行的事件
 * dropdownChange ：改变状态触发的事件
 * getGroupStatus() 获得状态值比如$('#toolbar').getGroupStatus() 
 */

;if (typeof Object.create !== 'function') {
    Object.create = function (obj) {
        function F() { }
        F.prototype = obj;
        return new F();
    };
}

(function ($, window, document, undefined) {
    var groupStatus = "";//选择状态值用于返回
    var ToolBar = {
        init: function (options, elem) {//初始化
            var self = this;
            self.elem = elem;
            self.$elem = $(elem);
            self.options = $.extend({}, $.fn.toolbar.options, options);
            self.$elem.css({
                    "color": "white",
                    "background-color": "#98bf21",
                    "font-family": "Arial",
                    "font-size": "20px",
                    "padding": "5px",
                    "position": "fixed",
                    "right": "0",
                    "left": "0",
                    "top":"0px",
                    "z-index": "1030"
            })
            self.selectedGroupID = 0;//选择的组ID
            self.initializeToolbar();
        },

        initializeToolbar: function () {//初始化工具条
            var self = this;
            self.loadDataSource();//加载数据
            //self.populateContent();
            //self.setTrigger();
        },

        loadDataSource: function () {//加载数据用
            var self = this;
            if (self.options.items) {//有数据源
                for (var i = 0; i < self.options.items.length; i++) {
                    self.$elem.append(self.addtoolbar(self.options.items[i], i));
                }
            }
            else if (self.options.treeId) {//有webapi获取数据
                postWebJson(self.options.controllerFunction + "?treeId=" + self.options.treeId, "",
                    function (resultdata) {
                        if (resultdata){
                            self.addtoolbar(resultdata);//生成TOOLBAR
                        }
                    })
            }
        },


        addtoolbar: function (items) {//toolbar由下拉框和按钮组一起组成，中华商务专用
            var self = this;
            var $toolbar = $('<div class="btn-toolbar" role="toolbar"></div>')
            var buttonGroup = {
                BG_Desc: "所有",
                BG_Id: 1,
                BG_Mark: "ALL",
                BG_Name: "全部",
                BG_OrderIndex: 1,
                BG_RowVersion: "AAAAAAAACmY="
            }
            var dropdown = $("<div id='dropdown_state' class='btn-group'></div>");//定义下了框，加载分类信息          
            
            dropdown.append("<button type='button' class='btn btn-primary'>" + items[0].BG_Name + "</button>");
            dropdown.append("<button type='button' class='btn btn-primary dropdown-toggle' data-toggle='dropdown'><span class='caret'></span></button>")
            dropdown.append("<ul class='dropdown-menu'></ul>")
            self.selectedGroupID = items[0].BG_Id;//当前选中的组ID
            groupStatus = items[0].BG_Mark;//状态
            var all_buttons = $("<div></div>");//全部按钮组
            for (var i = 0; i < items.length; i++) {//遍历回调回来的数据
                $.extend(buttonGroup, items[i]);
                var $li = $('<li></li>');
                $li.data("BG_Id", items[i].BG_Id);//对应组的ID号，好控制显示隐藏
                $li.data("BG_Mark", items[i].BG_Mark);//状态
                $li.on('click', self, self.dropdownChange);
                $li.append("<a href='#'>" + items[i].BG_Name + "</a>");
                dropdown.find('ul').append($li);//加载下拉选择项目
  //下面开始加载按钮组
                    var button = {
                        BTN_Desc: "修改",
                        BTN_Id: 4,
                        BTN_Mark: "EDIT",//事件名称
                        BTN_Name: "修改",
                        BTN_OrderIndex: 2,
                        BTN_RowVersion: "AAAAAAAACLU=",
                        CurrentBtnHasRights: true
                    }
                    var buttons = $("<div id='state_btn-group-" + items[i].BG_Id + "' class='btn-group'></div>");//按钮组
                    for (var j = 0; j < items[i].ButtonLst.length; j++) {
                        button = $.extend(button, items[i].ButtonLst[j]);//合并属性
                        var aa = { key: 'BTN_EVENT_' + self.options.treeId + "_" + items[i].BG_Mark + "_" + items[i].ButtonLst[j].BTN_Mark, _self: self }
                        var $button = $("<button type='button' class='btn btn-default'>" + items[i].ButtonLst[j].BTN_Name + "</button>")
                        if (button.CurrentBtnHasRights) {
                           // console.debug("1" + button.CurrentBtnHasRights);
                            $button.on('click', aa, self.toolBarBtnClick)//第三个参数定义了方法的参数用event.data可以取到
                        }
                        else {
                            console.debug("2" + button.CurrentBtnHasRights);
                            $button.attr("disabled", "disabled")
                        }//追加不可用样式

                        buttons.append($button);
                    }
                    if (i != 0)
                    { buttons.hide() }
                    all_buttons.append(buttons);
            }
            $toolbar.append(dropdown);
            $toolbar.append(all_buttons);
            self.$elem.append($toolbar);//加载下拉框

        },
        dropdownChange: function(event){
            var aa = $(this).text();
            var bb = event.data.selectedGroupID;//当前活动的组
            var cc = $(this).data("BG_Id");//之前存入的ID号，也就是现在选中的组id
            groupStatus = $(this).data("BG_Mark");//状态
            $('#dropdown_state button:first').text(aa);
            $("#state_btn-group-" + bb).hide();
            $("#state_btn-group-" + cc).show();
            event.data.selectedGroupID = cc;//改变状态ID
            if (event.data.options.dropdownChange)//如果定义了外部的选择事件
                event.data.options.dropdownChange(event.data);
        },
        toolBarBtnClick: function (event)
        {
            if (event.data._self.options.toolBarBtnClick)//如果定义了外部的选择事件
                event.data._self.options.toolBarBtnClick(event.data.key);
        },
        getGroupStatus: function ()
        {
            return groupStatus;
        }
    }

    $.fn.toolbar = function (options) {
        if ($.isPlainObject(options)) {
            return this.each(function () {
                var toolbarObj = Object.create(ToolBar);
                toolbarObj.init(options, this);
                $(this).append(toolbarObj);
            });
        } 
    };

    $.fn.toolbar.options = {
        webapiURL:  "http://localhost:40315/",
        controllerFunction: "Authority/GetButtonGroupHasRights",
        treeId: 18
    };

    $.fn.getGroupStatus = function () { // 返回toolbar组的状态

        return ToolBar.getGroupStatus();

    }; //end flexReload
})(jQuery, window, document);
