/* function uploadNow() { // onsubmit
    // Get form
    var form = $('#uploadAvatar')[0];

    // Create an FormData object 
    var data = new FormData(form);

    // If you want to add an extra field for the FormData
    data.append("CustomField", "This is some extra data, testing");

    // disabled the submit button
    $("#saveAvatar").prop("disabled", true);

    $.ajax({
        type: "POST",
        enctype: 'multipart/form-data',
        url: "/ManageAccount/UploadAvatar",
        data: data,
        processData: false,
        contentType: false,
        cache: false,
        timeout: 600000,
        success: function (data) {

            $("#uploadImageMessage").html(data);
            $("#saveAvatar").hide();

        },
        error: function (e) {

            alert("Have error!");

        }
    });
    return false;
} */

/* function uploadNow() {
    var formdata = new FormData(); //FormData object
    var fileInput = document.getElementById('imgInp');
    //Iterating through each files selected in fileInput
    for (i = 0; i < fileInput.files.length; i++) {
        //Appending each file to FormData object
        formdata.append(fileInput.files[i].name, fileInput.files[i]);
    }
    //Creating an XMLHttpRequest and sending
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/ManageAccount/UploadAvatar');
    xhr.send(formdata);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            alert("Uploaded successfully!");
            $("#uploadImageMessage").html(data);
            $("#saveAvatar").hide();
        }
    }
    return false;
}   */

function readURL(input) {
	if (input.files && input.files[0]) {
		var reader = new FileReader();

		reader.onload = function(e) {
			$('#blah').attr('src', e.target.result);
			$("#saveAvatar").show();
			$("#btnSave").show();
		}
		reader.readAsDataURL(input.files[0]);
	}
}

$("#imgInp").change(function() {
	readURL(this);
});
