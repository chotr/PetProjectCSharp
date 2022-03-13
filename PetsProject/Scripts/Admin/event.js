
function setListBill() {
	if ($("#changeListBill").val() == 'RECEIVED ORDER') {
		$("#listOrderBill").show();
		$("#listCancelledBill").hide();
		$("#listOnWayBill").hide();
		$("#listDeliveredBill").hide();
	} else if ($("#changeListBill").val() == 'CANCELLED') {
		$("#listOrderBill").hide();
		$("#listCancelledBill").show();
		$("#listOnWayBill").hide();
		$("#listDeliveredBill").hide();
	} else if ($("#changeListBill").val() == 'ON THE WAY') {
		$("#listOrderBill").hide();
		$("#listCancelledBill").hide();
		$("#listOnWayBill").show();
		$("#listDeliveredBill").hide();
	} else if ($("#changeListBill").val() == 'DELIVERED') {
		$("#listOrderBill").hide();
		$("#listCancelledBill").hide();
		$("#listOnWayBill").hide();
		$("#listDeliveredBill").show();
	}
}

function setListCmt() {
	if ($("#changeListCmt").val() == 'ACTIVE') {
		$("#activeCmt").show();
		$("#deactiveCmt").hide();
	} else if ($("#changeListCmt").val() == 'DEACTIVE') {
		$("#activeCmt").hide();
		$("#deactiveCmt").show();
	}
}

function changeStatusBill(idBill) {
	$("#buttonSaveStatusBill" + idBill).show();
	$("#checkSaveStatusBill" + idBill).hide();
	$("#buttonSendMail" + idBill).hide();

	$("#buttonSaveStatusBill" + idBill).val(
		$("#changeStatusBill" + idBill).val());

	var selected = $('#changeStatusBill' + idBill).children("option:selected").text();
	$("#buttonSendMail" + idBill).val(selected);
}

function changeStatusCmt(idBill) {
	$("#buttonSaveStatusCmt" + idBill).show();
	$("#checkSaveStatusCmt" + idBill).hide();
	

	$("#buttonSaveStatusCmt" + idBill)
			.val($("#changeStatusCmt" + idBill).val());
}

function saveStatusBill(idBill, statusBill) {

	var edit = {}
	edit["id"] = idBill;
	edit["status"] = statusBill;

	$.ajax({
		type : "POST",
		contentType : "application/json",
		url: '/Admin_Address/saveStatusBill',
		data: JSON.stringify(edit),
		dataType : 'json',
		cache : false,
		timeout : 600000,
		success : function(data) {

			$("#buttonSaveStatusBill" + idBill).hide();
			$("#checkSaveStatusBill" + idBill).show();
			$("#buttonSendMail" + idBill).show();
			$("#buttonSendMail" + idBill).prop('disabled', false);

		},
		error : function(e) {

			alert("Have failed!");

		}
	});

}

function sendMail(idBill, status) {
	var edit = {}
	edit["id"] = idBill;
	edit["status"] = status;

	$("#buttonSendMail" + idBill).html('<div class="spinner-border"></div>Sending...');
	$("#buttonSendMail" + idBill).prop('disabled', true);

	$.ajax({
		type: "POST",
		contentType: "application/json",
		url: '/Admin_Address/sendMail',
		data: JSON.stringify(edit),
		dataType: 'json',
		cache: false,
		timeout: 600000,
		success: function () {

			alert("Send mail success!");
			$("#buttonSendMail" + idBill).prop('disabled', false);
			$("#buttonSendMail" + idBill).html('Send mail again');
		},
		error: function (e) {

			alert("Have failed!");

		}
	});
}

function saveStatusCmt(idCmt, statusCmt) {

	var edit = {}
	edit["id"] = idCmt;
	edit["status"] = statusCmt;

	$.ajax({
		type : "POST",
		contentType : "application/json",
		url : "saveStatusCmt",
		data : JSON.stringify(edit),
		dataType : 'json',
		cache : false,
		timeout : 600000,
		success : function(data) {

			$("#buttonSaveStatusCmt" + idCmt).hide();
			$("#checkSaveStatusCmt" + idCmt).show();
		},
		error : function(e) {

			alert("Have failed!");

		}
	});

}

function saveStatusRepCmt(idCmt, statusCmt) {

	var edit = {}
	edit["id"] = idCmt;
	edit["status"] = statusCmt;

	$.ajax({
		type : "POST",
		contentType : "application/json",
		url : "saveStatusRepCmt",
		data : JSON.stringify(edit),
		dataType : 'json',
		cache : false,
		timeout : 600000,
		success : function(data) {

			$("#buttonSaveStatusCmt" + idCmt).hide();
			$("#checkSaveStatusCmt" + idCmt).show();
		},
		error : function(e) {

			alert("Have failed!");

		}
	});
}

function resetBill() {
	var r = confirm("This thing will remove all bill in list! Are you continue?");
	if (r == true) {
		var edit = {};
		$.ajax({
			type : "POST",
			contentType : "application/json",
			url : "deleteBill",
			data : JSON.stringify(edit),
			dataType : 'json',
			cache : false,
			timeout : 600000,
			success : function() {

				location.reload()

			},
			error : function() {

				alert("Have failed!");

			}
		});
	}
}
