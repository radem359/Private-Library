$(function () {

    $('#choose').click(function () {
        inHTML = "";
        $("#Authors option:selected").each(function () {
            inHTML += '<option value="' + $(this).val() + '">' + $(this).text() + '</option>';
        });
        $("#Authors option:selected").remove();
        $("#ChoosenAuthors").append(inHTML);
    });

    $("form").submit(function (e) {
        $("#ChoosenAuthors option").attr("selected", "selected");
    });

    $('#remove').click(function () {
        inHTML = "";
        $("#ChoosenAuthors option:selected").each(function () {
            inHTML += '<option value="' + $(this).val() + '">' + $(this).text() + '</option>';
        });
        $("#Authors").append(inHTML);
        $("#ChoosenAuthors option:selected").remove();
    });
});