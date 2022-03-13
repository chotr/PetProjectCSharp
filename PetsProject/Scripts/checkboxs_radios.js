// Xu ly su kien cho checkbox "Doc dieu khoan dich vu" trong trang checkout
$(document).ready(function() {
	$("#readPolicyCheckbox").click(function() {
		if ($(this).is(":checked")) {
			$("#payNowButton").attr("style","");
		} else if ($(this).is(":not(:checked)")) {
			$("#payNowButton").attr("style","cursor: not-allowed;opacity: 0.5;text-decoration: none;pointer-events: none;");
		}
	});
});

// Xu ly su kien cho radio Cash va PayPal
$(document).ready(function() {
	// Khi nhan vao radio "CASH"
	$("#f-option5").click(function() {
		if ($(this).is(":checked")) {
			$("#payNowButton").attr("data-target","#myModalCash");
		} 
	});
	// Khi nhan vao radio "PayPal"
	$("#f-option6").click(function() {
		if ($(this).is(":checked")) {
			$("#payNowButton").attr("data-target","#myModalPayPal");
		} 
	});
});

// Cap nhat noi dung trong note vao form "Buy Now!", Buy by PayPal
$( "#message" ).keyup(function() {
	var text=$("#message").val();
	 $(".noteBill").val(text);
	});