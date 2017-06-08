
///**
// * grid.js
// * Grid数据列表
// * @author        zj
// * @version       1.0.0
// * @requires      jQuery 1.9+ knockout 2.2+ knockout.mapping
// * @license 
// * Copyright 2013 中华商务联合印刷有限公司
// * 使用方法很简单
// * 1、<div id="grid" style="padding: 10px 10px 10px 10px;">
// * 2、var grid = $("#grid").grid({
//                dataSource: "Test/GetAttendanceByPage",//数据源地址
//                columns: [//显示列
//                      {
//                          field: "EM_Name_3",//字段
//                          title: "姓名"//列标题
//                      },
//                      {
//                          field: "EnrollNumber",
//                          title: "工号"
//                      },
//                      {
//                          field: "EnrollDateTime",
//                          title: "考勤时间",
//                          template: function (item) {//格式处理函数
//                              if (item.EnrollDateTime) {
//                                  var date = ConvertToDate(item.EnrollDateTime);
//                                  return date.Format("yyyy-MM-dd HH:mm:ss");
//                              }
//                          }
//                      },
//                      {
//                          field: "MachineNumber",
//                          title: "考勤机"
//                      }
//                ],
//                param: {
//                    staffID: "",
//                    startDate: "",
//                    endDate: ""
//                }
//            });

// * API 文档
// * 注意事项：
// *   var test = new fun(){
// *       this;//作用域this=test
// *   } 
// */

if (typeof Object.create !== 'function') {
    Object.create = function (obj) {
        function F() { }
        F.prototype = obj;
        return new F();
    };
}

(function ($, window, document, undefined) {
    var grid = {
        init: function (options, elem) {
            var self = this;
            self.$elem = $(elem);
            self.options = $.extend({}, $.fn.grid.options, options);
            self.dataSource = options.dataSource || "";
            self.param = $.extend({}, self.param, options.param);
            self.grid = $('<table class="grid"></table>')
                .appendTo(self.$elem);
            self.initializeGrid();
        },

        initializeGrid: function () {
            this.createTHead();
            this.createBody();
            this.getData();
        },

        dataSource: "",
        param: { pageIndex: 1, pageSize: 10 },

        createTHead: function () {
            var self = this;
            var thead = $("<thead></thead>");
            if (self.options.columns) {
                thead.append(self.addHeadRow(self.options.columns));
            }
            self.grid.append(thead);
        },

        addHeadRow: function (columns) {
            var self = this;
            var row = $("<tr></tr>");
            $(columns).each(function (index, item) {
                row.append(self.addHeadCell(item));
            });
            return row;
        },

        addHeadCell: function (column) {
            var cell = $("<th></th>").text(column.title);
            return cell;
        },

        createBody: function () {
            var self = this;
            var body = $("<tbody></tbody>");
            if (self.options.dataSource) {
                var data = self.options.dataSource;
                $(data).each(function (index, item) {
                    body.append(self.addRow(self.options.columns, item));
                });
            }
            self.grid.append(body);
        },

        addRow: function (columns, data) {
            var self = this;
            var row = $("<tr></tr>");
            $(columns).each(function (index, column) {
                row.append(self.addCell(column, data));
            });
            return row;
        },

        addCell: function (column, data) {
            var cell = $("<td></td>");
            if (data[column.field]) {
                cell.text(getValue(data, column.field));
            }
            return cell;
        },

        getValue: function (data, col) {//显示单元格数据
            if (data[col.field] != undefined) {
                if (col.template) {
                    return col.template(data);
                }
                return data[col.field];
            }
        },

        getData: function () {//获取数据
            var self = this;data
            //if (self.dataSource) {
            //    getWebJson(self.dataSource, self.param, function (data) {

            //    });
            //}
        }

    };
    $.fn.grid = function (options) {
        if ($.isPlainObject(options)) {
            var gridObj = Object.create(grid);
            gridObj.init(options, this);
            $(this).data('gridObj', gridObj);
        }
    }
})(jQuery, window, document);





//(function ($, window, document, undefined) {
//    var grid = {
//        init: function (options, elem) {
//            var self = this;
//            self.$elem = $(elem);
//            self.options = $.extend({}, $.fn.grid.options, options);
//            self.initializeGrid();
//        },

//        initializeGrid: function () {//初始化
//            var self = this;
//            self.getTemplate();//获取模板
//            self.viewModel = new self.model(self.options);
//            ko.applyBindings(self.viewModel);
//        },

//        getTemplate: function () {//获取模板
//            var self = this;
//            this.template = "../../../Scripts/CCSH.UI/grid/gridTemplate.html";
//            this.getTemplate = function () {
//                if (self.template) {
//                    $.ajax({
//                        async: false,
//                        url: self.template,
//                        type: 'GET',
//                        dataType: 'html',
//                        success: function (response) {
//                            self.$elem.append(response);
//                        },
//                        error: function (response) {
//                            var error = "没有找到模板：" + self.template;
//                            console.error(response);
//                        }
//                    });
//                }
//            };
//            this.getTemplate();
//        },

//        model: function (options) {//基础方法，创建grid时可被覆盖  
//            var self = this;
//            this.columns = options.columns || [];//显示列
//            this.dataSource = options.dataSource || null;//数据源
//            this.data = ko.mapping.fromJS([]);
//            this.getValue = function (data, col) {//显示单元格数据
//                if (data[col.field] != undefined) {
//                    if (col.template) {
//                        return col.template(ko.toJS(data));
//                    }
//                    return data[col.field];
//                }
//            };
//            this.selectedItem = ko.observable({});//选择的行
//            this.selectItem = function () {//选择行
//                self.selectedItem(this);
//            };
//            this.isSelected = function (item) {//行是否被选中
//                if (item == self.selectedItem()) {
//                    return true;
//                }
//                return false;
//            };
//            this.getNextPage = function () {//上一页
//                self.param.pageIndex++;
//                getData();
//            };
//            this.getPreviousPage = function () {//下一页
//                self.param.pageIndex--;
//                getData();
//            };
//            this.getPage = function (index) {//获取第index页
//                self.param.pageIndex = index - 1;
//                getData();
//            };
//            this.param = $.extend({ pageIndex: 1, pageSize: 10 }, options.param);//参数

//            var getData = function () {//获取数据
//                if (self.dataSource) {
//                    getWebJson(self.dataSource, self.param, function (data) {
//                        ko.mapping.fromJS(data.DataToRet, self.data);
//                    });
//                }
//            };
//            getData();//加载时获取第一页数据
//        },
//        getSelectedItem: function () {
//            return this.viewModel.selectedItem();
//        }

//    };
//    $.fn.grid = function (options) {
//        if ($.isPlainObject(options)) {
//            var gridObj = Object.create(grid);
//            gridObj.init(options, this);
//            $(this).data('gridObj', gridObj);
//            return gridObj;
//        }
//    }
//})(jQuery, window, document);