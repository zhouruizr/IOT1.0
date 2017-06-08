var specilflag = 0;
function popHandle(){
	var btn = $(".topfixed .share"),
		pop = $(".pop"),
		closeBtn = $(".pop .closeBtn img");
	btn.on("click",function(){
	    pop.removeClass("hide").addClass("show");
	    $("html,body").css("overflow","hidden");
	});
	closeBtn.on("click",function(){
	    pop.removeClass("show").addClass("hide");
	    if (specilflag==0) {
	        $("html,body").css("overflow", "visible");
	    } else {
	        return;
	    }
	});
}
function pageResize(){
	$(window).resize(function () {
      var isWidescreen = $(this).width();
      if (isWidescreen < 320) {
          isWidescreen = 320;
      } else if (isWidescreen > 640) {
          isWidescreen = 640;
      }
      var ratio = isWidescreen / 320;
      $("html").css({ 'font-size': ratio * 16 + "px" });
  }).trigger('resize');
  $("body").css("display", "block");
}
$(document).ready(function(){
	pageResize();
	popHandle();
});