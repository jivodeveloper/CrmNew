function Validatephonenumber(inputtxt) {
    var phoneno = /^\d{10}$/;
    if (inputtxt.value != "") {
        console.log(inputtxt.value);
        if (inputtxt.value.match(phoneno)) {
            $("#Errorphone").html("<font color='green'>Thank You</font>");
            return true;
        }
        else {
            $("#Errorphone").html("<font color='red'>Please Enter 10 digit Number only</font>");
            return false;
        }
    }
    else { $("#Errorphone").html("<font color='red'>Please Enter phone Number</font>"); }
}

function ValidateEmail(inputText) {
    var mailformat = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;
    if (inputText.value.match(mailformat)) {
        $("#ErrorEmail").html("<font color='Green'>Thank you</font>");

        return true;
    }
    else {
        $("#ErrorEmail").html("<font color='red'>Please Enter Correct Email</font>");
        return false;
    }
}

//validate Picode
function validateZipnumber(inputtxt) {
    console.log(inputtxt);
    var Zipno = /^\d{6}$/;
    if (inputtxt.value.match(Zipno)) {
        alert("correct");
        return true;
    }
    else {
        alert("Error");
        return false;
    }
}
//end

function AllAlphabet(inputtxt) {
    var letters = /^[A-Za-z]+$/;
    if (inputtxt.value.match(letters)) {
        $("#ErrorName").html("<font color='Green'>Thank You</font>");
        return true;
    }
    else {
        $("#ErrorName").html("<font color='Red'>Please Enter Alphabet Only</font>");
        return false;
    }
}

function SearchingwithPagination(table) {
    
    $(table).DataTable({
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select><option value=""></option></select>')
                    .appendTo($(column.footer()).empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        //to select and search from grid
                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });

                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        }
    });
}