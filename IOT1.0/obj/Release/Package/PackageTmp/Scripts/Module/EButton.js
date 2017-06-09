(function ($) {
    //按钮模型
    var EButton_mod = function () {
        var _EButton = this;
        _EButton.BTN_Id = "";
        _EButton.BTN_Name = "";
        _EButton.BTN_Name_En = "";
        _EButton.BTN_Mark = "";
        _EButton.BTN_Desc = "";
        _EButton.BTN_OrderIndex = "";

        //实体验证
        _EButton.BTN_Blur = function () {

            postWebJson("Button/ValBtnAttribute", ko.toJS(_EButton), function (data) {
                if (data == "0") {
                    msg.info("按钮名称不可重复！");
                    _EButton.BTN_Name("");
                }
                if (data == "1") {
                    msg.info("按钮编号不可重复！");
                    _EButton.BTN_Name_En("");
                }
                if (data == "2") {
                    msg.info("按钮序号不可重复！");
                    _EButton.BTN_OrderIndex(0);
                    return;
                }
                if (data == "3") {
                    msg.info("按钮标记不可重复！");
                    _EButton.BTN_Mark("");
                }
            });
        }
        //新增
        _EButton.Add = function (fn_onSucess, fn_onErr) {
            postWebJson("Button/Add", ko.toJS(_EButton), function (data) {
                if (fn_onSucess) {
                    msg.info("新增成功！");
                    fn_onSucess();

                }
            });
        }
        //修改
        _EButton.Edit = function (fn_onSucess, fn_onErr) {
            postWebJson("Button/Edit", ko.toJS(_EButton), function (data) {
                if (fn_onSucess) {
                    msg.info("保存成功！");
                    fn_onSucess();
                }
            });
        }
        //获取按钮，把返回的data赋给当前对象_EButton
        _EButton.Get = function (fn_onSucess, fn_onErr) {
            postWebJson("Button/Get", ko.toJS(_EButton), function (data) {
                ko.mapping.fromJS(data, {}, _EButton);
            });
        }
        //删除按钮
        _EButton.Delete = function (fn_onSucess, fn_onErr) {
            postWebJson("Button/Delete", ko.toJS(_EButton), function (data) {
                if (fn_onSucess) {
                    msg.info("删除成功！");
                    fn_onSucess();
                }
            });
        }
    }
}(jQuery));