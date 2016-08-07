var popWin = {
	scrolling: "auto",
	"int": function() {
		this.mouseClose(),
		this.closeMask()
	},
	showWin: function(a, b, c, d) {
		var e = b - 52,
		f = a / 2,
		g = b / 2,
		h = "";
		h += '<div id="mask" style="width:100%; height:100%; position:fixed; top:0; left:0; z-inde:1999;background:#cccccc; filter:alpha(opacity=10); -moz-opacity:0.1; -khtml-opacity: 0.1; opacity:0.1;"></div>',
		h += '<div id="maskTop" style="width: ' + a + "px; height: " + b + "px; border: #d1d1d1 5px solid; background: #ffffff; color: #333; position: fixed; top: 50%; left: 50%; margin-left: -" + f + "px; margin-top: -" + g + 'px; z-index: 2999; border-radius:5px; ">',
		h += '<div id="maskTitle" style="height:35px; line-height: 35px; font-family:ËÎÌå; font-size: 14px; font-weight:bold; color: #797979; padding-left: 10px; background: url(./images/hbbg.png); border-bottom: 1px solid #d1d1d1; position: relative;">',
		h += "" + c,
		h += '<div id="popWinClose" style="width: 15px; height:15px; cursor: pointer; position: absolute; top:10px; right: 9px; background: url(./images/closehdtipper.png)"></div>',
		h += "</div>",
		h += '<iframe width="' + a + '" height="' + e + '" frameborder="0" scrolling="' + this.scrolling + '" src="' + d + '"></iframe>',
		$("body").append(h),
		this.int()
	},
	mouseClose: function() {
		$("#popWinClose").on("mouseenter",
		function() {
			$(this).css("background-image", "url(./images/closehdtipper.png)")
		}),
		$("#popWinClose").on("mouseleave",
		function() {
			$(this).css("background-image", "url(./images/closehdtipper.png)")
		})
	},
	closeMask: function() {
		$("#popWinClose").on("click",
		function() {
			$("#mask,#maskTop").fadeOut(function() {
				$(this).remove()
			})
		})
	}
};
//·µ»Ø¶¥²¿
function b() {
	h = $(window).height(),
	t = $(document).scrollTop(),
	t > h ? $("#moquu_top").show() : $("#moquu_top").hide()
}
$(document).ready(function() {
	b(),
	$("#moquu_top").click(function() {
		$(document).scrollTop(0)
	})
}),
$(window).scroll(function() {
	b()
});