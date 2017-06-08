if (typeof Object.create !== 'function') {
    Object.create = function (obj) {
        function F() { }
        F.prototype = obj;
        return new F();
    };
}

var page = {
    init: function (options, elem) {
        var self = this;
        self.$elem = $(elem);
        self.options = $.extend({}, options);
        return self.initializeGrid();
    },
    initializeGrid: function () {
        var grid = new this.grid();
        var pagination = new this.pagination();
        
        var viewModel = $.extend({}, grid, pagination);
        return viewModel;
    },
    grid: function () {
        var self = this;
        this.columns = [
            {
                field: "EM_Name_3",
                title: "姓名"
            },
            {
                field: "EnrollNumber",
                title: "工号"
            },
            {
                field: "EnrollDateTime",
                title: "考勤时间",
                template: function (item) {
                    if (item.EnrollDateTime) {
                        var date = ConvertToDate(item.EnrollDateTime);
                        return date.Format("yyyy-MM-dd HH:mm:ss");
                    }
                }
            },
            {
                field: "MachineNumber",
                title: "考勤机"
            }
        ];
        this.dataSource = "Test/GetAttendanceByPage";
        this.pageCount = ko.observable(0);
        this.data = ko.mapping.fromJS([]);
        this.getValue = function (data, col) {//显示单元格数据
            if (data[col.field] != undefined) {
                if (col.template) {
                    return col.template(ko.toJS(data));
                }
                return data[col.field];
            }
        };
        this.selectedItem = ko.observable({});//选择的行
        this.selectItem = function () {//选择行
            self.selectedItem(this);
        };
        this.isSelected = function (item) {//行是否被选中
            if (item == self.selectedItem()) {
                return true;
            }
            return false;
        };
        this.getPage = function (index) {//获取第index页
            self.param.pageIndex = index - 1;
            getData();
        };
        this.param = {
            pageIndex: 1, pageSize: 10,
            staffID: "",
            startDate: "",
            endDate: ""
        };//参数

        var getData = function () {//获取数据
            if (self.dataSource) {
                getWebJson(self.dataSource, self.param, function (data) {
                    ko.mapping.fromJS(data.DataToRet, self.data);
                    self.pageCount(data.AllPages);
                });
            }
        };
        getData();//加载时获取第一页数据
    },
    pagination: function () {
        var self = this;
        this.pageCount = ko.observable(0);
        this.groupSize = 10;
        this.pageItems = ko.observableArray([]);
        this.groupIndex = ko.observable(1);
        this.pageIndex = ko.observable(1);

        this.gotoPreviousPage = function () {//上一页
            if (!self.isFirstPage()) {
                self.pageIndex(this.pageIndex() - 1);
                pageIndexOverRange();
            }
        };
        this.gotoNextPage = function () {//下一页
            if (!self.isLastPage()) {
                self.pageIndex(this.pageIndex() + 1);
                pageIndexOverRange();
            }
        };
        this.gotoFirstPage = function () {//首页
            self.pageIndex(1);
            pageIndexOverRange();
        };
        this.gotoLastPage = function () {//末页
            self.pageIndex(self.pageCount);
            pageIndexOverRange();
        };
        this.gotoPage = function () {//跳转页
            self.pageIndex(this);
        };
        this.gotoPrePageGroup = function () {//上一组
            self.groupIndex(self.groupIndex() - 1);
            groupIndexChange();
        };
        this.gotoNextPageGroup = function () {//下一组
            self.groupIndex(self.groupIndex() + 1);
            groupIndexChange();
        };
        this.isFirstGroupPage = function () {//当前是第一组
            if (self.groupIndex() == 1) {
                return false;
            }
            return true;
        };
        this.isLastGroupPage = function () {//当前是最后组
            var lastGroup = parseInt(self.pageCount / self.groupSize) + (self.pageCount % self.groupSize == 0 ? 0 : 1);
            if (self.groupIndex() == lastGroup) {
                return false;
            }
            return true;
        };
        this.isFirstPage = ko.computed(function () {//是否为首页
            if (self.pageIndex() == 1) {
                return true;
            }
            return false;
        }, this);
        this.isLastPage = ko.computed(function () {//是否为末页
            if (self.pageIndex() == self.pageCount) {
                return true;
            }
            return false;
        }, this);


        var setPageItems = function () {//设置页码
            var myArray = [];
            var temp = (self.groupIndex() - 1) * self.groupSize + 1;
            myArray = createPageArray(temp);
            self.pageItems([]);
            $(myArray).each(function (index, item) {
                self.pageItems.push(item);
            });
        };
        var createPageArray = function (start) {//创建数组
            var myArray = [];
            for (var i = 0; i < self.groupSize; i++) {
                var temp = start + i;
                if (temp > 0 && temp <= self.pageCount) {
                    myArray.push(temp);
                } else {
                    break;
                }
            }
            return myArray;
        };
        var pageIndexOverRange = function () {//页码超出当前页码数组，则重置它
            if (self.pageItems().indexOf(self.pageIndex()) <= 0) {
                setGroupIndexByPageIndex();
                setPageItems();
            }
        };
        var setGroupIndexByPageIndex = function () {//根据页码设置组码
            var temp = parseInt(self.pageIndex() / self.groupSize) + (self.pageIndex() % self.groupSize == 0 ? 0 : 1);
            self.groupIndex(temp);
        };
        var groupIndexChange = function () {//组码改变
            setPageItems();
            self.pageIndex(self.pageItems()[0]);
        };
        var setPageIndexByGroupIndex = function () {//根据组码设置页码
            var temp = (self.groupIndex() - 1) * self.groupSize + 1;
            self.pageIndex(temp);
        };
        setPageItems();
    }
}

//(function ($) {
//    var pagination = {
//        init: function (options, elem) {
//            var self = this;
//            self.$elem = $(elem);
//            self.options = $.extend({}, $.fn.pagination.options, options);
//            self.initializeGrid();
//        },
//        initializeGrid: function () {//初始化
//            var self = this;
//            self.getTemplate();//获取模板
//            self.pagViewModel = new self.model(self.options);
//            ko.applyBindings(self.viewModel);
//        },
//        getTemplate: function () {//获取模板
//            var self = this;
//            this.template = "../../../Scripts/CCSH.UI/grid/pagination.html";
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
//            this.pageCount = options.pageCount || 0;
//            this.groupSize = options.groupSize || 0;
//            this.pageItems = ko.observableArray([]);
//            this.groupIndex = ko.observable(1);
//            this.pageIndex = ko.observable(1);

//            this.gotoPreviousPage = function () {//上一页
//                if (!self.isFirstPage()) {
//                    self.pageIndex(this.pageIndex() - 1);
//                    pageIndexOverRange();
//                }
//            };
//            this.gotoNextPage = function () {//下一页
//                if (!self.isLastPage()) {
//                    self.pageIndex(this.pageIndex() + 1);
//                    pageIndexOverRange();
//                }
//            };
//            this.gotoFirstPage = function () {//首页
//                self.pageIndex(1);
//                pageIndexOverRange();
//            };
//            this.gotoLastPage = function () {//末页
//                self.pageIndex(self.pageCount);
//                pageIndexOverRange();
//            };
//            this.gotoPage = function () {//跳转页
//                self.pageIndex(this);
//            };
//            this.gotoPrePageGroup = function () {//上一组
//                self.groupIndex(self.groupIndex() - 1);
//                groupIndexChange();
//            };
//            this.gotoNextPageGroup = function () {//下一组
//                self.groupIndex(self.groupIndex() + 1);
//                groupIndexChange();
//            };
//            this.isFirstGroupPage = function () {//当前是第一组
//                if (self.groupIndex() == 1) {
//                    return false;
//                }
//                return true;
//            };
//            this.isLastGroupPage = function () {//当前是最后组
//                var lastGroup = parseInt(self.pageCount / self.groupSize) + (self.pageCount % self.groupSize == 0 ? 0 : 1);
//                if (self.groupIndex() == lastGroup) {
//                    return false;
//                }
//                return true;
//            };
//            this.isFirstPage = ko.computed(function () {//是否为首页
//                if (self.pageIndex() == 1) {
//                    return true;
//                }
//                return false;
//            }, this);
//            this.isLastPage = ko.computed(function () {//是否为末页
//                if (self.pageIndex() == self.pageCount) {
//                    return true;
//                }
//                return false;
//            }, this);


//            var setPageItems = function () {//设置页码
//                var myArray = [];
//                var temp = (self.groupIndex() - 1) * self.groupSize + 1;
//                myArray = createPageArray(temp);
//                self.pageItems([]);
//                $(myArray).each(function (index, item) {
//                    self.pageItems.push(item);
//                });
//            };
//            var createPageArray = function (start) {//创建数组
//                var myArray = [];
//                for (var i = 0; i < self.groupSize; i++) {
//                    var temp = start + i;
//                    if (temp > 0 && temp <= self.pageCount) {
//                        myArray.push(temp);
//                    } else {
//                        break;
//                    }
//                }
//                return myArray;
//            };
//            var pageIndexOverRange = function () {//页码超出当前页码数组，则重置它
//                if (self.pageItems().indexOf(self.pageIndex()) <= 0) {
//                    setGroupIndexByPageIndex();
//                    setPageItems();
//                }
//            };
//            var setGroupIndexByPageIndex = function () {//根据页码设置组码
//                var temp = parseInt(self.pageIndex() / self.groupSize) + (self.pageIndex() % self.groupSize == 0 ? 0 : 1);
//                self.groupIndex(temp);
//            };
//            var groupIndexChange = function () {//组码改变
//                setPageItems();
//                self.pageIndex(self.pageItems()[0]);
//            };
//            var setPageIndexByGroupIndex = function () {//根据组码设置页码
//                var temp = (self.groupIndex() - 1) * self.groupSize + 1;
//                self.pageIndex(temp);
//            };
//            setPageItems();
//        },
//        refresh: function (options) {//刷新
//            console.debug(this.viewModel);
//            this.pagViewModel.pageCount = options.pageCount;
//            this.pagViewModel.gotoFirstPage();
//        }
//    };
//    $.fn.pagination = function (options) {
//        if ($.isPlainObject(options)) {
//            var pagObj = Object.create(pagination);
//            pagObj.init(options, this);
//            $(this).data('pagObj', pagObj);
//            return pagObj;
//        }
//    }
//})(jQuery);