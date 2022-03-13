$(document).ready(function() {

	$("#postReview").submit(function(event) {

		event.preventDefault();

		postReview_ajax_submit();

	});

	$("#postComment").submit(function(event) {

		event.preventDefault();

		postComment_ajax_submit();

	});

	$("#addToCartForm").submit(function(event) {

		event.preventDefault();

		addToCard_ajax_submit();

	});

	$("#deleteTable").submit(function(event) {

		event.preventDefault();

		deleteTable_ajax_submit();

	});

	$("#editBasicInfoForm").submit(function(event) {

		event.preventDefault();

		editBasicInfo_ajax_submit();

	});

	$("#sendMail").submit(function(event) {

		event.preventDefault();

		sendMail_ajax_submit();

	});

	$("#uploadAvatar").submit(function (event) {

		event.preventDefault();

		uploadAvatar_ajax_submit();

	});


});


function addToCard_ajax_submit() {

	var add = {}
	add["id"] = $("#idItem").val();

	$.ajax({
		type : "POST",
		contentType : "application/json",
		url : "/addToCartAJAX",
		data : JSON.stringify(add),
		dataType : 'json',
		cache : false,
		timeout : 600000,
		success : function(data) {

			var json = JSON.stringify(data, null, 4);
			var result = JSON.parse(json);
			$('#lblCartCount').html(result.quantityCart);
			$('#checkoutBtn').show();
			alert("Add to cart successfully!");

		},
		error : function(e) {

			alert("Add to cart failed!");

		}
	});

}

function postReview_ajax_submit() {

	var add = {}
	add["username"] = $("#usernameForm").val();
	add["id"] = $("#idItem").val();
	add["content"] = $("#contentBoxReview").val();
	add["rate"] = $("#numRate").children("option:selected").val();
	$
			.ajax({
				type : "POST",
				contentType : "application/json",
				url : "postReviewAJAX",
				data : JSON.stringify(add),
				dataType : 'json',
				cache : false,
				timeout : 600000,
				success : function(data) {
					
					var json = JSON.stringify(data, null, 4);
					var result = JSON.parse(json);
					
					var text="";
					var i;
					for (i = 0; i < result.rate; i++) {
					  text += '<i class="fa fa-star"></i>';
					}
					
					$("#listReviews")
							.append(
									'<div class="review_item"><div class = "media"><div class= "d-flex"><img style="width:60px;height:60px" src='
											+ result.imageslink
											+ ' ></div><div class="media-body"><h4>'
											+ result.fullname
											+ '</h4>'+text+'</div></div><p>'
											+ result.content + '</p></div>');
					$("#contentBoxReview").val('');

				},
				error : function(e) {

					alert("Posting review failed!");

				}
			});

}

function postComment_ajax_submit() {

	var add = {}
	add["username"] = $("#usernameForm").val();
	add["id"] = $("#idItem").val();
	add["content"] = $("#contentBoxComment").val();
	$
			.ajax({
				type : "POST",
				contentType : "application/json",
				url : "postCommentAJAX",
				data : JSON.stringify(add),
				dataType : 'json',
				cache : false,
				timeout : 600000,
				success : function(data) {

					var json = JSON.stringify(data, null, 4);
					var result = JSON.parse(json);

					var today = new Date();
					var date = today.getDate() + '-' + (today.getMonth() + 1)
							+ '-' + today.getFullYear() + "  "
							+ today.getHours() + ":" + today.getMinutes();
					$("#listComments")
							.append(
									'<div class="review_item"><div class="media"><div class="d-flex"><img style="width:60px;height:60px" src="'
											+ result.imageslink
											+ '" alt=""></div><div class="media-body"><h4>'
											+ result.fullname
											+ '</h4><h5>'
											+ date
											+ '</h5></div></div><p>'
											+ result.content + '</p></div>');
					$("#listComments")
							.append(
									'<div class="review_item reply"><div class="media"><div class="media-body"><div class="input-group mb-3"><input type="text" id="replyCmtBox" class="repcmt'
											+ result.idComment
											+ '" style="width: 100%;" placeholder="Reply this comment..." autofocus><div class="input-group-append" style="margin: right;"><button type="button" class="btn primary-btn" onclick="repCmt(value)" value="'
											+ result.idComment
											+ '" autofocus>Reply</button></div></div></div></div></div>');
					$("#contentBoxComment").val("");

				},
				error : function(e) {

					alert("Posting comment failed!");

				}
			});

}

function deleteTable_ajax_submit() {

	var product = {}
	product["id"] = "abcd";

	$.ajax({
		type : "POST",
		contentType : "application/json",
		url : "deleteCart",
		data : JSON.stringify(product),
		dataType : 'json',
		cache : false,
		timeout : 600000,
		error : function(e) {

			$('#lblCartCount').html(0);
			$('#checkoutBtn').hide();
			$('#totalCost').html("0");
			$('#emptyCart').show();
			$('.entryCart').hide();

		}
	});

}

function editBasicInfo_ajax_submit() {

	var edit = {};
	edit["username"] = $("#username").val();
	edit["fullname"] = $("#fullname").val();
	edit["phone"] = $("#phone").val();
	edit["email"] = $("#email").val();
	edit["address"] = $("#address").val();

	$.ajax({
		type : "POST",
		contentType : "application/json",
		url: '/ManageAccount/EditBasicInfo',
		data: JSON.stringify(edit),
		dataType : 'json',
		cache : false,
		timeout : 600000,
		success : function(data) {

			var json = JSON.stringify(data, null, 4);
			var result = JSON.parse(json);
			$("#showFullname").html(result.fullname);
			$("#showPhone").html(result.phone);
			$("#showEmail").html(result.email);
			$("#showAddress").html(result.address);

			$("#basicInfo").show();
			$("#basicInfoEdit").hide();

		},
		error : function(e) {

			alert("Edit info failed!");

		}
	});

}

function setAddressDefault(idAddress) {
	var set = {};
	set["idAddressOld"] = $("#idAddressDefault").val();
	set["idAddressNew"] = idAddress;

	$
			.ajax({
				type : "POST",
				contentType : "application/json",
				url : "/ManageAccount/EditDefaultAddress",
				data : JSON.stringify(set),
				dataType : 'json',
				cache : false,
				timeout : 600000,
				success : function(data) {

					var json = JSON.stringify(data, null, 4);
					var result = JSON.parse(json);
					alert("Change info successful!");
					$("#statusAB" + result.idAddressNew).html('DEFAULT');
					$("#statusAB" + result.idAddressOld)
							.html(
									'<button type="button" class="btn btn-warning" name="'
											+ result.idAddressOld
											+ '" onclick="setAddressDefault(name)">Set Default</button>');
					$("#idAddressDefault").val(idAddress);

				},
				error : function(e) {

					alert("Change info failed!");

				}
			});
}

function sendEmailSignup() {
	var set = {};
	set["email"] = $("#email").val();

	$.ajax({
		type : "POST",
		contentType : "application/json",
		url : "sendEmailSignup",
		data : JSON.stringify(set),
		dataType : 'json',
		cache : false,
		timeout : 600000,
		success : function(data) {

			var json = JSON.stringify(data, null, 4);
			var result = JSON.parse(json);
			$("#hiddenCode").val(result.code);
			$("#showEmail").html($("#email").val());
			alert("fjewkgdwiugbd")

		},
		error : function(e) {

			alert("Sending email failed!");

		}
	});
}

function checkAvailableUsername() {
	var set = {};
	set["text"] = $("#usernameField").val();

	$
			.ajax({
				type : "POST",
				contentType : "application/json",
				url : "checkAvailableUsername",
				data : JSON.stringify(set),
				dataType : 'json',
				cache : false,
				timeout : 600000,
				success : function(data) {
					var json = JSON.stringify(data, null, 4);
					var result = JSON.parse(json);
					$("#errorUsername").html(result.content);
					if (result.content == "The username is already exists!")
						$("#registerButton")
								.attr("style",
										"cursor: not-allowed;opacity: 0.5;text-decoration: none;pointer-events: none;");
					if (result.content == "")
						$("#registerButton").attr("style", "");

				},
				error : function(e) {

					alert("Checking username failed!");

				}
			});
}

function uploadAvatar_ajax_submit() {
	var set = {};
	var form = $('#uploadAvatar')[0]; 
	var formData = new FormData(form);

	$.ajax({
		type: "POST",
		enctype: 'multipart/form-data',
		contentType: false,
		processData: false,
		url: "/ManageAccount/UploadAvatar",
		data: formData,
		cache: false,
		timeout: 600000,
		success: function (data) {

			$("#uploadImageMessage").html(data);
			$("#saveAvatar").hide();

		},
		error: function (e) {

			alert("Upload failed!");

		}
	});
}

function savePassword() {
	var set = {};
	set["newPassword"] = $('#newpass').val();
	set["oldPassword"] = $('#oldpass').val();
	$.ajax({
		type: "POST",
		contentType: "application/json",
		url: "SaveNewPassword",
		data: JSON.stringify(set),
		dataType: 'json',
		cache: false,
		timeout: 600000,
		success: function (data) {
			if (data == 'The old password is incorrect!') {
				alert("The old password is incorrect!");
			}
			else if (data == 'Saving password successfully!') {
				alert("Saving password successfully!");
				$("#signinInfo").show();
				$("#siginInfoEdit").hide();
			}
		},
		error: function () {

			alert("Have failed!");

		}
	});
}

function checkPasswordToDeleteAccount_ajax_submit(){
	var set = {};
	set["password"] = $('#passwordToDeleteAccount').val();
	$('#validateBtn').hide();
	
	$.ajax({
		type: "POST",
		contentType: "application/json",
		url: "CheckPasswordToDeleteAccount",
		data: JSON.stringify(set),
		dataType: 'json',
		cache: false,
		timeout: 600000,
		success: function (data) {
			if (data == 'incorrect') {
				$('#messagePassword').html("The password is incorrect!");
				$('#messagePassword').css("color", "red");
				$('#sendMyRequestBtn').prop('disabled', true);
				$('#validateBtn').show();
			}
			else if (data == 'correct') {
				$('#messagePassword').html("The password is correct! Please click Send my request button to send your request of remove your account to admin!");
				$('#messagePassword').css("color", "green");
				$('#sendMyRequestBtn').prop('disabled', false);
				$('#validateBtn').show();
			}
		},
		error: function () {

			alert("Have failed!");

		}
	});
}

function sendRequestToDeleteAccount() {
	var set = {};
	$('#sendMyRequestBtn').prop('disabled',true);
	$('#messageSendRequest').html('Sending request...');
	$.ajax({
		type: "POST",
		contentType: "application/json",
		url: "SendRequestToDeleteAccount",
		data: JSON.stringify(set),
		dataType: 'json',
		cache: false,
		timeout: 600000,
		success: function (data) {
			$('#messageSendRequest').html('Your request was sent!');
		},
		error: function () {

			alert("Have failed!");

		}
	});
}



