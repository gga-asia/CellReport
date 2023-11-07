

function buildLanguageFile() {
    //抓取掛有多國標籤的tag 取出key 值, 組出Json 字串

    var linkstring = "{";

    $("[data-i18n-text]").each(function () {
        var length = $("[data-i18n-text]").length;

        linkstring += '"';

        linkstring += $.trim( $(this).text());

        linkstring += '":"';

        linkstring += $.trim($(this).text());

        linkstring += '",';




    });

    linkstring += "}";
    console.log(linkstring);
}
function applyI18n() {
    var isInput = $(this).is("input");
    var isPlaceholder = false;
    var msg = isInput ? $.trim($(this).val()) : $.trim($(this).text());
    if (msg === "" && $.trim($(this).attr("placeholder")) !== "") {
        isPlaceholder = true;
        msg = $(this).attr("placeholder");
    }
    var localeMsg = $.i18n(msg);

    $(this).removeAttr("data-i18n-text").attr("data-i18n-done", "");
    if (!isPlaceholder) {
        isInput ? $(this).val(localeMsg) : $(this).text(localeMsg);
    } else {
        $(this).attr("placeholder", localeMsg);
    }
}