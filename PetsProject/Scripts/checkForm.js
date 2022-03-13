$(document).ready(function() {
	$("#usernameField").keyup(function(event) {
		$("#errorUsername").html('');
	});

	$("#password").keyup(function(event) {
		$("#errorPassword").html('');
	});

	$("#rePassword").keyup(function(event) {
		$("#errorRePassword").html('');
	});
	$("#phone").keyup(function(event) {
		$("#errorPhone").html('');
	});
	$("#email").keyup(function(event) {
		$("#errorEmail").html('');
	});
});


function validation() {
	deleteError();
	var username = $("#usernameField").val();
	var pass1 = $("#password").val();
	var pass2 = $("#rePassword").val();
	var fullname = $("#fullname").val();
	var birthday = $("#fullname").val();
	var phone = $("#phone").val();
	var email = $("#email").val();
	var address = $("#address").val();
	var mailformat = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
	var phoneformat = /[0-9]{10}/;
	var result = true;
	if (username.length < 8) {
		$("#errorUsername").html('Username must be at least 8 characters!');
		result = false;
	}
	if (pass1.length < 8) {
		$("#errorPassword").html('Password must be at least 8 characters!');
		result = false;
	}
	if (pass1 != pass2) {
		$("#password").val("");
		$("#rePassword").val("");
		$("#errorPassword").html('Password is not match!');
		$("#errorRePassword").html('Password is not match!');
		result = false;
	}

	if (!(email.match(mailformat))) {
		$("#errorEmail").html("Email must be example123@abc.com!");
		result = false;
	}

	if (!(phone.match(phoneformat))) {
		$("#errorPhone").html("Phone numbers include only 10 digits!");
		result = false;
	}

	if (!result)
		return false;
}

function deleteError() {
	$("#errorUsername").html('');
	$("#errorPassword").html('');
	$("#errorRePassword").html('');
	$("#errorPhone").html('');
	$("#errorEmail").html('');
}