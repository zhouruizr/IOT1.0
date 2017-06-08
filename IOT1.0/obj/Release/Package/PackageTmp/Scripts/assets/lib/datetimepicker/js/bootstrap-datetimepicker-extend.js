/* =========================================================
 * bootstrap-datetimepicker-extend.js
 * =========================================================
 *  显示已存在的日期控件
 *  重写place方法，允许根据传入的控件定位
 *  
 *  example:
 *  picker.datetimepicker_show(this);
 *
 *
 *
 *
*/
$.fn.datetimepicker_show = function (/*显示在控件*/target) {
    return this.each(function () {
        var $this = $(this),
                data = $this.data('datetimepicker');
        data.place = function () {
            if (this.isInline) return;

            var index_highest = 0;
            $('div').each(function () {
                var index_current = parseInt($(this).css("zIndex"), 10);
                if (index_current > index_highest) {
                    index_highest = index_current;
                }
            });
            var zIndex = index_highest + 10;

            var offset, top, left;
            if (this.component) {
                offset = this.component.offset();
                left = offset.left;
                if (this.pickerPosition == 'bottom-left' || this.pickerPosition == 'top-left') {
                    left += this.component.outerWidth() - this.picker.outerWidth();
                }
            } else {
                if (target) {
                    offset = $(target).offset();
                } else {
                    offset = this.element.offset();
                }
                left = offset.left;
            }
            if (this.pickerPosition == 'top-left' || this.pickerPosition == 'top-right') {
                top = offset.top - this.picker.outerHeight();
            } else {
                top = offset.top + this.height;
            }
            this.picker.css({
                top: top,
                left: left,
                zIndex: zIndex
            });
        };
        data.show();
    });
}