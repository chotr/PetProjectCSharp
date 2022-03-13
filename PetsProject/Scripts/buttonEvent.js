function setNewDefaultAddress() {
	var set = {};
	set["address"] = $('#newAddressText').val();
	var text = $('#newAddressText').val();
	$.ajax({
		type : "POST",
		contentType : "application/json",
		url : "addNewAddress_SetDefault",
		data : JSON.stringify(set),
		dataType : 'json',
		cache : false,
		timeout : 600000,
		success : function(data) {
			var json = JSON.stringify(data, null, 4);
			var result = JSON.parse(json);
			$("#deliveryAddress").html(text);
			$(".idAddressBook").val(result.idAddressNew);
			$('#newAddressText').val('');
		},
		error : function() {

			alert("Have failed!");

		}
	});

}

function setCurrentAddress() {
	var value = $("#listAddressSelect option:selected");
	$("#deliveryAddress").html(value.text());
	$(".idAddressBook").val(value.val());
}

function editBasicInfo() {
	$("#basicInfo").hide();
	$("#basicInfoEdit").show();
}

//function editSigninInfo() {
//	$("#signinInfo").hide();
//	$("#siginInfoEdit").show();
//}

function returnBack() {
	$("#basicInfoEdit").hide();
	$("#siginInfoEdit").hide();
	$("#basicInfo").show();
	$("#signinInfo").show();
	return false;
}

function addNewAddress() {
	var addressText = prompt("Please new address you want to add: ", "");
	if (!(addressText == null || addressText == "")) {
		var address = {};
		address["address"] = addressText;
		$.ajax({
			type : "POST",
			contentType : "application/json",
			url : "/ManageAccount/AddAddressBook",
			data : JSON.stringify(address),
			dataType : 'json',
			cache : false,
			timeout: 600000,
			success: function (data) {
				$('#tableAddressBook').append(
					'<tr><td>' + addressText + '</td ><td id="statusAB' + data + '"><button type="button" class="btn btn-warning" name="' + data + '"onclick="setAddressDefault(name)">Set Default</button></td></tr >');
			},
			error: function () {
				alert("Have error!")
            }
		});
	}
}

function deleteDeliveredBillConfirm() {
	if (confirm("Are you want to remove history bill?")) {
		$.ajax({
			type : "POST",
			url: "/ManageAccount/DeleteDeliveredBill",
			cache : false,
			timeout : 600000,
			success : function(data) {
				$("#deliveredBillShow").hide();
				$("#checkNumDeliveredBill").hide();
				$("#deliveredBillEmpty").html('History is empty!');

			},
			error : function(e) {

				alert("Have failed!");

			}
		});
	}
}

function deleteCancelledBillConfirm() {
	if (confirm("Are you want to remove history bill?")) {
		$.ajax({
			type: "POST",
			url: "/ManageAccount/DeleteCancelledBill",
			cache: false,
			timeout: 600000,
			success: function (data) {
				$("#cancelledBillShow").hide();
				$("#checkNumCancelledBill").hide();
				$("#cancelledBillEmpty").html('History is empty!');

			},
			error: function (e) {

				alert("Have failed!");

			}
		});
	}
}

function uploadAvatar() {
	var set = {};

	$.ajax({
		type: "POST",
		contentType: "application/json",
		url: '@Url.Action("SaveAvatar")',
		dataType: 'json',
		cache: false,
		timeout: 600000,
		success : function(data) {
			
			$("#uploadImageMessage").html(data);
			$("#saveAvatar").hide();

		},
		error : function(e) {

			alert("Upload failed!");

		}
	});
}


