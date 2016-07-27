var Authenticode = {
    getAuthenticodeButtonId: false,
    againGetAuthenticodeTimeId: false,
    getAuthenticodeButtonText: false,
    getAuthenticodeUrl: false,
    isPreventing: false, //是否正处于阻止获取状态
    againGetAuthenticodeSpacingTime: 300, //重新获取验证码间隔时间
    preventOverCallback: function () { },

    init: function (getAuthenticodeButtonId,
        againGetAuthenticodeTimeId,
		getAuthenticodeButtonText,
		getAuthenticodeUrl,
        againGetAuthenticodeSpacingTime) {

        this.getAuthenticodeButtonId = getAuthenticodeButtonId;
        this.againGetAuthenticodeTimeId = againGetAuthenticodeTimeId;
        this.getAuthenticodeButtonText = getAuthenticodeButtonText;
        this.getAuthenticodeUrl = getAuthenticodeUrl;
        if (againGetAuthenticodeSpacingTime != null && typeof (againGetAuthenticodeSpacingTime) != "undefined")
            this.againGetAuthenticodeSpacingTime = againGetAuthenticodeSpacingTime;

        //$("#" + Authenticode.getAuthenticodeButtonId).attr("disabled", "disabled");
    },

    getAuthenticode: function (data) {
        //$("#" + this.getAuthenticodeButtonId).val("正在获取，请稍后...");
        //$("#" + this.getAuthenticodeButtonId).attr("disabled", "disabled");
        $("#" + this.getAuthenticodeButtonId).hide();
        $("#" + this.againGetAuthenticodeTimeId).html("正在获取，请稍后...");
        $("#" + this.againGetAuthenticodeTimeId).show();

        $.ajax({
            cache: false,
            type: "POST",
            url: Authenticode.getAuthenticodeUrl,
            data: data,
            success: function (d) {
                if (d.Data.Success) {
                    alert(d.Data.Message);
                    Authenticode.preventInterval();
                }
                else {
                    alert(d.Data.Message);
                    Authenticode.enableButton();
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('Failed to retrieve states.');
                Authenticode.enableButton();
            }
        });
    },

    preventInterval: function () {
        this.isPreventing = true;
        var time = this.againGetAuthenticodeSpacingTime;
        interval = setInterval(function () {
            var btnText = Authenticode.getAuthenticodeButtonText.replace("{0}", time);
            //$("#" + Authenticode.getAuthenticodeButtonId).val(btnText);
            $("#" + Authenticode.getAuthenticodeButtonId).hide();
            $("#" + Authenticode.againGetAuthenticodeTimeId).html(btnText);
            $("#" + Authenticode.againGetAuthenticodeTimeId).show();
            if (time == 0) {
                clearInterval(interval);
                Authenticode.isPreventing = false;
                //if (Authenticode.preventOverCallback != null && Authenticode.preventOverCallback)
                //    Authenticode.preventOverCallback();
                Authenticode.enableButton();
            }
            time--;
        }, 1000);
    },

    enableButton: function () {
        if (!this.isPreventing) {
            //$("#" + Authenticode.getAuthenticodeButtonId).removeAttr("disabled");
            //$("#" + Authenticode.getAuthenticodeButtonId).val("获取验证码");

            $("#" + this.getAuthenticodeButtonId).show();
            $("#" + this.againGetAuthenticodeTimeId).html("");
            $("#" + this.againGetAuthenticodeTimeId).hide();
        }
    },

    disableButton: function () {
        if (!this.isPreventing) {
            //$("#" + Authenticode.getAuthenticodeButtonId).attr("disabled", "disabled");
            //$("#" + Authenticode.getAuthenticodeButtonId).val("获取验证码");

            $("#" + this.getAuthenticodeButtonId).hide();
            $("#" + this.againGetAuthenticodeTimeId).html("获取验证码");
            $("#" + this.againGetAuthenticodeTimeId).show();
        }
    }
};
