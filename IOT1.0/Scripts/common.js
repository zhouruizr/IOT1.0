//两列自定义绑定
{
    ko.bindingHandlers.comboBox_Options = {//绑定CombBox数据
        init: function (element, valueAccessor, allBindingsAccessor, viewModal) {
            var value = valueAccessor();
            var valueUnwrapped = ko.unwrap(value);
            $(element).ComboBox({
                data: valueUnwrapped,
                display: [
                    { text: "名称", value: "name" },
                    { text: "编号", value: "code" }
                ],
                displayValue: "name",
                value: "code",
                twidth: '116px'
            });
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModal) {
            var value = valueAccessor();
            var valueUnwrapped = ko.unwrap(value);
            $(element).Reload({ data: valueUnwrapped });    // 重新绑定option数据
            allBindingsAccessor.get("comboBox_Value")("");  // option绑定数据变化时，清除combobox value绑定值
        }
    }

    // knockout 自定义combobox值绑定
    ko.bindingHandlers.comboBox_Value = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModal) {
            var values = valueAccessor();
            $(element).change(function (e, value, data) {   // 设置变更事件
                if (value) {
                    values(value);
                }
                else {
                    values("");
                }
            });
        },
        update: function (element, valueAccessor) {
            if ($(element).attr("type") == "ComboBox") {
                var value = valueAccessor();
                var valueUnwrapped = ko.unwrap(value);
                var selectValue = $(element)[0].setValue(valueUnwrapped);   // 根据value值设置combobox选项值
            }
        }
    }

    // knockout 自定义combobox文本绑定
    ko.bindingHandlers.comboBox_Text = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModal) {
            var values = valueAccessor();
            $(element).change(function (e, value, data) {   // 设置变更事件
                if (value) {
                    values($(e.target).find('input').val())
                }
                else {
                    values("");
                }
            });
        }
    }
}

//一列自定义绑定
{
    ko.bindingHandlers.comboBox_OptionsOne = {//绑定CombBox数据
        init: function (element, valueAccessor, allBindingsAccessor, viewModal) {
            var value = valueAccessor();
            var valueUnwrapped = ko.unwrap(value);
            $(element).ComboBox({
                data: valueUnwrapped,
                display: [
                    { text: "名称", value: "name" }
                ],
                displayValue: "name",
                value: "name",
                twidth: '116px'
            });
        },
        update: function (element, valueAccessor, allBindingsAccessor, viewModal) {
            var value = valueAccessor();
            var valueUnwrapped = ko.unwrap(value);
            $(element).Reload({ data: valueUnwrapped });    // 重新绑定option数据
            allBindingsAccessor.get("comboBox_ValueOne")("");  // option绑定数据变化时，清除combobox value绑定值
        }
    }

    // knockout 自定义combobox值绑定
    ko.bindingHandlers.comboBox_ValueOne = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModal) {
            var values = valueAccessor();
            $(element).change(function (e, value, data) {   // 设置变更事件
                if (value) {
                    values(value);
                }
                else {
                    values("");
                }
            });
        },
        update: function (element, valueAccessor) {
            if ($(element).attr("type") == "ComboBox") {
                var value = valueAccessor();
                var valueUnwrapped = ko.unwrap(value);
                var selectValue = $(element)[0].setValue(valueUnwrapped);   // 根据value值设置combobox选项值
            }
        }
    }

    // knockout 自定义combobox文本绑定
    ko.bindingHandlers.comboBox_TextOne = {
        init: function (element, valueAccessor, allBindingsAccessor, viewModal) {
            var values = valueAccessor();
            $(element).change(function (e, value, data) {   // 设置变更事件
                if (value) {
                    values($(e.target).find('input').val())
                }
                else {
                    values("");
                }
            });
        }
    }
}

$(document).ready(function () {
    {
       

        var datetimepicker_date = $("<input id='template_date' class='form-control' type='text' style='display:none' />");
        $("body").append(datetimepicker_date);
        $("#template_date").datetimepicker({
            language: 'zh-CN',
            weekStart: 1,
            todayBtn: 1,
            autoclose: 1,
            todayHighlight: 1,
            startView: 2,
            minView: 2,
            forceParse: 0,
            format: "yyyy-mm-dd"
        });

        var datetimepicker_time = $("<input id='template_time' class='form-control' type='text' style='display:none' />");
        $("body").append(datetimepicker_time);
        $("#template_time").datetimepicker({
            language: 'zh-CN',
            autoclose: 1,
            startView: 1,
            format: "hh:ii",
            minuteStep: 30//间隔分钟
        });

        var datetimepicker_datetime = $("<input id='template_datetime' class='form-control' type='text' style='display:none' />");
        $("body").append(datetimepicker_datetime);
        $("#template_datetime").datetimepicker({
            language: 'zh-CN',
            weekStart: 1,
            todayBtn: 1,
            autoclose: 1,
            todayHighlight: 1,
            startView: 2,
            minView: 0,
            forceParse: 0,
            format: "yyyy-mm-dd hh:ii:00",
            minuteStep: 30
        });

        $("#template_date").on("change", function () {
            var op = $($("#template_date").data("op"));
            op.val($(this).val());
            op.change();
        });

        $("#template_time").on("change", function () {
            var op = $($("#template_time").data("op"));
            op.val($(this).val());
            op.change();
        });

        $("#template_datetime").on("change", function () {
            var op = $($("#template_datetime").data("op"));
            op.val($(this).val());
            op.change();
        });

        ko.bindingHandlers.date = {//自定义绑定，用于绑定日期控件
            init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                var value = valueAccessor();
                var valueUnwrpped = ko.unwrap(value);
                $(element).focus(function () {
                    var datetime = $("#template_date");
                    datetime.data("op", this);
                    datetime.datetimepicker_show(this);
                });
                $(element).change(function () {
                    value($(this).val());
                });
            },
            update: function (element, valueAccessor) {
                var value = valueAccessor();
                var valueUnwrpped = ko.unwrap(value);
                $(element).val(valueUnwrpped);
            }
        }

        ko.bindingHandlers.time = {//自定义绑定，用于绑定时间控件
            init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                $(element).focus(function () {
                    var datetime = $("#template_time");
                    datetime.data("op", this);
                    datetime.datetimepicker_show(this);
                });
                var value = valueAccessor();
                $(element).change(function () {
                    value($(this).val());
                });
            },
            update: function (element, valueAccessor) {
                var value = valueAccessor();
                var valueUnwrpped = ko.unwrap(value);
                $(element).val(valueUnwrpped);
            }
        }

        ko.bindingHandlers.datetime = {//自定义绑定，用于绑定时间控件
            init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                $(element).focus(function () {
                    var datetime = $("#template_datetime");
                    datetime.data("op", this);
                    datetime.datetimepicker_show(this);
                });
                var value = valueAccessor();
                $(element).change(function () {
                    value($(this).val());
                });
            },
            update: function (element, valueAccessor) {
                var value = valueAccessor();
                var valueUnwrpped = ko.unwrap(value);
                $(element).val(valueUnwrpped);
            }
        }
    }
});


//日期减掉两天的函数（为了符合审核逻辑，把周日放到周五）
var datedifftoAudit = function (time) {
    var date = new Date(String(time));
    if (date.getDay() == 0) {
        date.setDate(date.getDate() - 2);
        date = date.Format("yyyy-MM-dd");
        return date;
    } else {
        return time;
    }
}



