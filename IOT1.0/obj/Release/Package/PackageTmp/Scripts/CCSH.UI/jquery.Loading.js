/*
 * jquery.Loading.js
 * 加载中插件
 * @author        张晶
 * @version       1.0.0
 * @requires      jQuery 1.9+ , jquery.control.css
 * @license 
 * Copyright 2013 中华商务联合印刷有限公司
 * 使用方法很简单
 * 将遮盖层遮盖整个页面
 *    var loading=$.loading();
 *loading.show();/loading.hide();
 * API 文档
 */
(function ($) {
    function Loading(container) {
        var loading;
		var _html = $("<div class='loading' style='display:none'></div>");
		//if (container) {
		//	var c = $("<div style=\"width:100%;height:100%;position:relative;\"></div>");
		//	c.append(_html);
		//	$(container).append(c);
		//} else {
		//	$("body").append(_html);
	    //}
		if (top != self) {
		    // alert('我在框架里');
		    $("html", window.parent.document).append(_html);
		}
		else {
		    $(document).ready(function(){
		        $("body").append(_html);
		    })
		}
		this.show = function () {
			$(_html).show();
		};
		this.hide = function () {
			$(_html).hide();
		};
	};
	$.loading = function (container) {
	    loading = new Loading(container);
		return loading;
	};
	$.fn.loading = function () {
	    return loading || $.loading(this);//zr修改，防止重复创建遮罩 
	};
})(jQuery);