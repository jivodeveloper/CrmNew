var SalespersonId = "", daysPresent = "", totalsalary = "", ModifiedtotalSalary = "", DailyAllowance = "", ModifiedDailyAllowance = "", SetDailyAllowances = "";
var salarydate = "", modified = "false", soname = "", basicsalry = "", Internet = "", Mobile = "", SalaryMonth = "", SalaryYear = "", Email = "", objnew = "", grossSalary = "";
var ta = "", res = "", dayscalulation = "", currentdays = "", dailyallowancecalc = "", onedaysalary = "", persentdaysalary = "", Modifieddayspresent = "", dayinmonth = "";
var digit = "", sumsalary = 0, sumLtrs = 0, totalsumsalary = 0, totalsumLtrs = 0, sumgrosssalary = 0, sumda = 0, sumta = 0, totalsumgrosssalary = 0, totalsumda = 0;
var totalsumta = 0, sumincentive = 0, totalsumincentive = 0, summobile = 0, totalsummobile = 0, suminternet = 0, totalsuminternet = 0, targetltr = 0, sumtargetltr = 0, totaltargetltr = 0;
var sumNewShopcount = 0, totalsumNewShopcount = 0;
var personType = "";
$(function () {
    $("#popup").dialog({
        autoOpen: false,
        title: "SALARY  FORM",
        height: 550,
        width: 800,

    });
});
function OpenPopUp() {
    $("#popup").dialog('open');
}
// Send Email pdf format popup
function EmailPopupNew(fileName, obj) {
    objnew = obj;
    $("#emailPopupNew").dialog({
        autoOpen: false,
        height: 350,
        width: 400,
    });
    $('#fileName').val(fileName);
    $("#emailPopupNew").dialog("open");
}
//send EmailAll pdf formats
function emailAllPopup(fileName, obj) {
    objnew = obj;
    $("#emailAllPopup").dialog({
        autoOpen: false,
        height: 350,
        width: 400,
    });
    $('#fileName').val(fileName);
    $("#emailAllPopup").dialog("open");
}
// When Click Submit button First Time Then Call Submit Function
function Submit() {
    // alert('aa')
    var fromDate = $('#fromDate').val();
    salarydate = fromDate;
    var stateId = $("#stateId").val();
    personType = $("#personType").val();
    //var statename = $("#stateId").text();
    var statename = $("#stateId option:selected").text();
    if ((fromDate != null && fromDate != "")) {
        if (personType != "--SELECT--") {
            $('#loader').show();
            $.ajax({
                url: '/Salary/GetSalaryReport',
                data: { fromDate: fromDate, personType: personType, stateId: stateId },
                success: function (data) {
                    
                    $("#grid").empty();
                    if (data.length > 0) {
                        totalsumgrosssalary = 0;
                        totalsumda = 0;
                        totalsumta = 0;
                        totalsummobile = 0;
                        totalsuminternet = 0;
                        totalsumincentive = 0;
                        totalsumsalary = 0;
                        totalsumLtrs = 0;
                        totaltargetltr = 0;
                        totalsumNewShopcount = 0;
                        $("#grid").append("<tr class='info'><th scope='col'>Name</th><th scope='col'>Basic Salary</th><th scope='col'>Days Present</th><th scope='col'>Gross Salary</th><th scope='col'>D.A</th><th scope='col'>Internet</th><th scope='col'>Mobile</th><th scope='col' style='display:none'>Total Ltrs.</th><th scope='col' style='display:none'>Cost  Per Ltrs.</th><th scope='col' style='display:none'>D.A  Per Ltrs.</th><th scope='col'>T.A </th><th scope='col' style='display:none'>T.A.Per Ltrs</th><th scope='col'>Incentive</th><th scope='col'>Arrears</th><th scope='col'>Remarks</th><th scope='col'>Net Salary</th><th scope='col'>Action</th></tr>");
                        for (var i = 0; i < data.length; i++) {
                            
                            SalaryMonth = ('' + data[i]["salaryMonth"] + '');
                            SalaryYear = ('' + data[i]["salaryYear"] + '');
                            sumgrosssalary = data[i]["grossSalary"];
                            totalsumgrosssalary = parseFloat(totalsumgrosssalary) + parseFloat(sumgrosssalary)
                            sumda = data[i]["dailyAllowance"];
                            totalsumda = parseFloat(totalsumda) + parseFloat(sumda)
                            sumta = data[i]["ta"];
                            totalsumta = parseFloat(totalsumta) + parseFloat(sumta)
                            summobile = data[i]["mobile"];
                            totalsummobile = parseFloat(totalsummobile) + parseFloat(summobile)
                            suminternet = data[i]["internet"];
                            totalsuminternet = parseFloat(totalsuminternet) + parseFloat(suminternet)
                            sumincentive = data[i]["incentive"];
                            totalsumincentive = parseFloat(totalsumincentive) + parseFloat(sumincentive)
                            sumsalary = data[i]["totalSalary"];
                            totalsumsalary = parseFloat(totalsumsalary) + parseFloat(sumsalary);
                            sumLtrs = data[i]["TotalLtrs"];
                            totalsumLtrs = parseFloat(totalsumLtrs) + parseFloat(sumLtrs);
                            sumtargetltr = data[i]["TargetLtrs"];
                            totaltargetltr = parseFloat(totaltargetltr) + parseFloat(sumtargetltr);
                            sumNewShopcount = data[i]["NewShopCount"];
                            totalsumNewShopcount = parseFloat(totalsumNewShopcount) + parseFloat(sumNewShopcount);
                            var Edit = '';
                            var View = '';
                            var button = '';
                            var Email = '';
                            if (data[i]["flag"] == 0) {
                                  Edit = "<i class='fa fa-pencil' style='font-size:120%;' onclick=\"ClickEdit('" + data[i]["personId"] + "')\" ></i>"
                                  Email = "<i class='fa fa-envelope' style='font-size:120%;' disabled ></i>"
                                  View = "<i class='fa fa-file-text' style='font-size:120%;' onclick=\"ClickView(this)\" ></i>"
                           //     Edit = "<core-icon icon='create' style='cursor: pointer;' onclick=\"ClickEdit('" + data[i]["personId"] + "')\" ></core-icon>"
                           //     Email = "<core-icon icon='mail' style='cursor: pointer;'disabled ></core-icon>"
                           //     View = "<core-icon icon='assignment' style='cursor: pointer;' onclick=\"ClickView(this)\" ></core-icon>"
                            }
                            else {
                                Edit = "<i class='fa fa-check' style='font-size:120%;' disabled ></i> | <i class='fa fa-pencil' style='font-size:120%;' onclick=\"ClickEdit('" + data[i]["personId"] + "')\" ></i>"
                                Email = "<i class='fa fa-envelope' style='font-size:120%;' onclick=\"EmailPopupNew('Salary Report',this)\" ></i>"
                                View = "<i class='fa fa-file-text' style='font-size:120%;' onclick=\"ClickView(this)\" ></i>"
                           //     Edit = "<core-icon icon='check' style='cursor: pointer;'disabled ></core-icon> | <core-icon icon='create' style='cursor: pointer;' onclick=\"ClickEdit('" + data[i]["personId"] + "')\" ></core-icon>"
                           //    //    Edit = "<core-icon icon='check' style='cursor: pointer;'disabled ></core-icon>"
                           //     Email = "<core-icon icon='mail' style='cursor: pointer;' onclick=\"EmailPopupNew('Salary Report',this)\" ></core-icon>"
                           //     View = "<core-icon icon='assignment' style='cursor: pointer;' onclick=\"ClickView(this)\" ></core-icon>"
                            }
                            $("#grid").append("<tr><td id='soname'>" +
                           data[i]["personName"] + "</td> <td id='contact' style='display:none'>" + data[i]["contactNO"] + "</td><td id='Email' style='display:none'>" + data[i]["email"] + "</td><td id='basicsalary'>" + data[i]["basicSalary"] + "</td><td id='dayspresent'>" + data[i]["daysPresent"] + "</td><td id='grossSalary'>" + data[i]["grossSalary"] + "</td><td id='setdailyallowances' style='display:none'>" + data[i]["setDailyAllowances"] + "</td><td id='daysForDA' style='display:none'>" + data[i]["daysForDA"] + "</td><td id='dailyallowance'>" + data[i]["dailyAllowance"] + "</td><td id='Internet'>" + data[i]["internet"] + "</td><td id='mobile'>" + data[i]["mobile"] + "</td><td id='TargetLtrs' style='display:none;'>" + data[i]["TargetLtrs"] + "</td><td id='NewShopCount' style='display:none;'>" + data[i]["NewShopCount"] + "</td><td id='TotalLtrs' style='display:none'>" + data[i]["TotalLtrs"] + "</td><td id='costPerLtrs' style='display:none'>" + data[i]["costPerLtrs"] + "</td><td id='daPerLtrs' style='display:none'>" + data[i]["daPerLtrs"] + "</td><td id='ta'>" + data[i]["ta"] + "</td><td id='taPerLtrs' style='display:none'>" + data[i]["taPerLtrs"] + "</td><td id='incentive'>" + data[i]["incentive"] + "</td><td id='arrear'>" + data[i]["arrear"] + "</td><td id='remark'>" + data[i]["remark"] + "</td><td id='totalsalry'>" + data[i]["totalSalary"] + "</td><td style='width:150px;'>" + Edit + '  |  ' + View + ' | ' + Email + "</td></tr>")
                        }

                        $('#loader').fadeOut();
                        $('#download').show();
                        $("#EmailAll").show();
                        $("#totalcalc").prop('value', "Total Salary (Rs.)  " + "" + totalsumsalary + "");
                        $('#totalcalc').show();
                    }

                    else {
                        $('#loader').fadeOut();
                        ShowMessage(personType + ' Are Not Avaliable In ! '  + statename, 'warning');
                        $("#grid").empty();
                        $("#grid").append("<tr class='info'><th scope='col'>Name</th><th scope='col'>Basic Salary</th><th scope='col'>Days Present</th><th scope='col'>Gross Salary</th><th scope='col'>D.A</th><th scope='col'>Internet</th><th scope='col'>Mobile</th><th scope='col' style='display:none'>Total Ltrs.</th><th scope='col' style='display:none'>Cost  Per Ltrs.</th><th scope='col' style='display:none'>D.A  Per Ltrs.</th><th scope='col'>T.A </th><th scope='col' style='display:none'>T.A.Per Ltrs</th><th scope='col'>Incentive</th><th scope='col'>Arrears</th><th scope='col'>Remarks</th><th scope='col'>Net Salary</th><th scope='col'>Action</th></tr>");

                    }
                }
            });
        }
        else {
            alert('Please Select PersonType');
        }
    }
    else {
        alert('Please Select Date ');
    }
}
//Edit Salary Form open in popup
function ClickEdit(PersonId) {
    var dayspresentForDa = '';
    SalespersonId = PersonId;
    var designation = $('#personType').val();
    $('#gridNew').show();
    $('#salaryslip').hide();
    $.ajax({
        url: '/Salary/GetEditData',
        data: { PersonId: PersonId },
        success: function (data) {
            
            daysPresent = ('' + data["daysPresent"] + '');
            totalsalary = ('' + data["totalSalary"] + '');
            DailyAllowance = ('' + data["dailyAllowance"] + '');
            SetDailyAllowances = ('' + data["setDailyAllowances"] + '');
            dayspresentForDa = ('' + data["daysForDA"] + '');
            basicsalry = ('' + data["basicSalary"] + '');
            Internet = ('' + data["internet"] + '');
            Mobile = ('' + data["mobile"] + '');
            grossSalary = ('' + data["grossSalary"] + '');
            ta = ('' + data["ta"] + '');
            costPerLtrs = ('' + data["costPerLtrs"] + '');
            daPerLtrs = ('' + data["daPerLtrs"] + '');
            taPerLtrs = ('' + data["taPerLtrs"] + '');
            TotalLtrs = ('' + data["TotalLtrs"] + '');
            incentive = ('' + data["incentive"] + '');
            var arrear = ('' + data["arrear"] + '');
            var remark = ('' + data["remark"] + '');
            targetltr = ('' + data["TargetLtrs"] + '');
            var dailyallowancecalc = ((SetDailyAllowances) + "*" + (dayspresentForDa))
            dayinmonth = ('' + data["thisMonthDays"] + '');
            $("#gridNew").empty();
            $("#salaryslip").empty();
            if (data != "") {
                $("#gridNew").append("<tr><td>" + designation + ":</td><td id='personname'>" + data["personName"] + "</td></tr>" +
                                      "<tr><td>Days Present:</td><td>" + "<input type='text' maxlength='2'  id='days' value='" + data["daysPresent"] + "' onkeypress='return event.charCode >= 48 && event.charCode <= 57' onkeyup='salarycalculation(" + data["thisMonthDays"] + ")'    >" + "</td></tr>" +
                                       "<tr><td>Days Present For DA:</td><td>" + "<input type='text' maxlength='2'  id='daysforda' value='" + data["daysForDA"] + "' onkeypress='return event.charCode >= 48 && event.charCode <= 57' onkeyup='dacalculation(" + data["thisMonthDays"] + ")'    >" + "</td></tr>" +
                                      "<tr><td>Daily Allowance (Rs.):</td><td id='DailyAllowance'>" + data["dailyAllowance"] + "(" + "" + dailyallowancecalc + ")" + "</td></tr>" +
                                      "<tr><td>Basic Salary(Rs.):</td><td id='bsalary'>" + data["basicSalary"] + "</td></tr><tr><td>Gross Salary (Rs.):</td><td id='gsalary'>" + data["grossSalary"] + "</td></tr>" +
                                      "<tr><td>Internet (Rs.).</td><td id='Internet'>" + data["internet"] + "</td></tr><tr><td>Mobile (Rs.):</td><td id='Mobile'>" + data["mobile"] + "</td></tr>" +
                                      "<tr><td>T.A. (Rs.)</td><td id='taallowance'>" + data["ta"] + "</td></tr><tr><td>Incentives (Rs.):</td><td>" + "<input type='text' id='commission' value='" + data["incentive"] + "' onkeypress='return event.charCode >= 48 && event.charCode <= 57' onkeyup='Incentive()'>" + "</td></tr>" +
                                      "<tr><td>Arrears (Rs.):</td><td>" + "<input type='text' id='arrears' value='" + data["arrear"] + "' onkeypress='return event.charCode >= 48 && event.charCode <= 57' onkeyup='Arrears()'>" + "</td></tr>" +
                                      "<tr><td>Remarks:</td><td>" + "<textarea  id='remark' value='" + data["remark"] + "' />" + "</td></tr>" +
                                      "<tr><td>Total Salary (Rs.)</td><td id='totalsalary'>" + data["totalSalary"] + "</td></tr>" +
                                      "<tr><td>Target Ltrs.</td><td id='targetltr'>" + data["TargetLtrs"] + "</td></tr>" +
                                      "<tr><td>Total Ltrs. Achived</td><td id='totalltrs'>" + data["TotalLtrs"] + "</td></tr><tr><td>Cost Per Ltrs (Rs.):</td><td id='costLtrs'>" + data["costPerLtrs"] + "</td></tr>" +
                                       "<tr><td>D.A Per Ltrs (Rs.).</td><td id='daLtrs'>" + data["daPerLtrs"] + "</td></tr><tr><td>T.A Per Ltrs (Rs.):</td><td id='taPLtrs'>" + data["taPerLtrs"] + "</td></tr>" +
                                       "<tr><td></td><td><input id='btnsubmit' type='submit' onclick='SaveSalary()' /></td></tr>");

            }
            OpenPopUp();
            $('#loader').fadeOut();

        },
        error: function () {
            alert('Error');
        }
    });
}
function Incentive() {
    
    var incentive = $('#commission').val();
    if (incentive == "") {
        incentive = 0;
    }
    var arrear = $('#arrears').val();
    if (arrear == "") {
        arrear = 0;
    }
    Modifieddayspresent = $('#days').val();
    comission = incentive;
    daysinmonth = dayinmonth;
    DailyAllowance;
    Internet
    Mobile
    var daily = $('#DailyAllowance').text();
    var dailyallowances = (parseInt(daily));
    basicsalry = $('#bsalary').text();
    onedaysalary = ((basicsalry) / (daysinmonth));
    persentdaysalary = Math.round((onedaysalary) * parseFloat(Modifieddayspresent));
    var taallowances = $('#taallowance').text();
    ModifiedtotalSalary = parseFloat((persentdaysalary) + parseFloat(dailyallowances) + parseFloat(Internet) + parseFloat(Mobile) + parseFloat(comission) + parseFloat(arrear) + parseFloat(taallowances));
    $('#totalsalary').text(parseFloat(ModifiedtotalSalary));
    var totalltrs = $('#totalltrs').text();
    if (totalltrs != 0) {
        var costPerLtrs = (ModifiedtotalSalary / totalltrs).toFixed(3);
        $('#costLtrs').text(costPerLtrs);
        var daltrs = (dailyallowances / totalltrs).toFixed(3);
        $('#daLtrs').text(daltrs);
        var talptrs = (taallowances / totalltrs).toFixed(3);
        $('#taPLtrs').text(talptrs);
    }

}
function Arrears() {
    
    var incentive = $('#commission').val();
    if (incentive == "") {
        incentive = 0;
    }
    var arrear = $('#arrears').val();
    if (arrear == "") {
        arrear = 0;
    }
    Modifieddayspresent = $('#days').val();
    comission = incentive;
    daysinmonth = dayinmonth;
    DailyAllowance;
    Internet
    Mobile
    var daily = $('#DailyAllowance').text();
    var dailyallowances = (parseInt(daily));
    basicsalry = $('#bsalary').text();
    onedaysalary = ((basicsalry) / (daysinmonth));
    persentdaysalary = Math.round((onedaysalary) * parseFloat(Modifieddayspresent));
    var taallowances = $('#taallowance').text();
    ModifiedtotalSalary = parseFloat((persentdaysalary) + parseFloat(dailyallowances) + parseFloat(Internet) + parseFloat(Mobile) + parseFloat(comission) + parseFloat(arrear) + parseFloat(taallowances));
    $('#totalsalary').text(parseFloat(ModifiedtotalSalary));
    var totalltrs = $('#totalltrs').text();
    if (totalltrs != 0) {
        var costPerLtrs = (ModifiedtotalSalary / totalltrs).toFixed(3);
        $('#costLtrs').text(costPerLtrs);
        var daltrs = (dailyallowances / totalltrs).toFixed(3);
        $('#daLtrs').text(daltrs);
        var talptrs = (taallowances / totalltrs).toFixed(3);
        $('#taPLtrs').text(talptrs);
    }
}
//Salary Calculation Function
function dacalculation(daysinmonth) {
    var dayscalulation = $('#daysforda').val();
    if (dayscalulation == "") {
        dayscalulation = 0;
    }
    Modifieddayspresent = dayscalulation;
    if (daysinmonth >= parseFloat(Modifieddayspresent)) {
        ModifiedDailyAllowance = ((SetDailyAllowances) * parseFloat(Modifieddayspresent));
        dailyallowancecalc = ((SetDailyAllowances) + "*" + parseFloat(Modifieddayspresent))
        var grosssalary = $('#gsalary').text();
        var Internet = $('#Internet').text();
        var Mobile = $('#Mobile').text();
        var taallowance = $('#taallowance').text();
        var incentivevalue = $('#commission').val();
        if (incentivevalue == "") {
            incentivevalue = 0;
            incentive = 0;
        }
        else {
            incentive = incentivevalue;
        }
        var arrear = $('#arrears').val();
        if (arrear == "") {
            arrear = 0;
        }
        ModifiedtotalSalary = (parseFloat(grosssalary) + parseFloat(ModifiedDailyAllowance) + parseFloat(Internet) + parseFloat(Mobile) + parseFloat(incentive) + parseFloat(arrear) + parseFloat(taallowance));
        $('#DailyAllowance').text(ModifiedDailyAllowance + "(" + dailyallowancecalc + ")");
        $('#totalsalary').text(ModifiedtotalSalary);
        var totalltrs = $('#totalltrs').text();
        var salary = $('#totalsalary').text();
        if (totalltrs != 0) {
            var costPerLtrs = (salary / totalltrs).toFixed(3);
            $('#costLtrs').text(costPerLtrs);
            var daltrs = (ModifiedDailyAllowance / totalltrs).toFixed(3);
            $('#daLtrs').text(daltrs);
            var talptrs = (taallowances / totalltrs).toFixed(3);
            $('#taPLtrs').text(talptrs);
        }
    }
    else {
        ShowMessage("'This Month has only '" + daysinmonth + "' Days'", "error")
        $('#daysforda').val("0")
    }
}
function salarycalculation(daysinmonth) {
    
    var dayscalulation = $('#days').val();
    var digit = dayscalulation.toString()[0];
    if (dayscalulation == "") {
        dayscalulation = 0;
    }
    Modifieddayspresent = dayscalulation;
    if (daysinmonth >= parseFloat(Modifieddayspresent)) {
        DailyAllowance;
        Internet
        Mobile
        onedaysalary = ((basicsalry) / (daysinmonth));
        persentdaysalary = Math.round((onedaysalary) * parseFloat(Modifieddayspresent));
        $('#gsalary').text(persentdaysalary);
        var daily = $('#DailyAllowance').text();
        var dailyallowances = (parseInt(daily));
        var incentivevalue = $('#commission').val();
        if (incentivevalue == "") {
            incentivevalue = 0;
            incentive = 0;
        }
        else {
            incentive = incentivevalue;
        }
        var arrear = $('#arrears').val();
        if (arrear == "") {
            arrear = 0;
        }
        var taallowances = $('#taallowance').text();
        ModifiedtotalSalary = parseFloat((persentdaysalary) + parseFloat(dailyallowances) + parseFloat(Internet) + parseFloat(Mobile) + parseFloat(incentive) + parseFloat(arrear) + parseFloat(taallowances))
        modified = "true";
        $('#totalsalary').text(ModifiedtotalSalary);
        var totalltrs = $('#totalltrs').text();
        var salary = $('#totalsalary').text();
        if (totalltrs != 0) {
            var costPerLtrs = (salary / totalltrs).toFixed(3);
            $('#costLtrs').text(costPerLtrs);
            var daltrs = (dailyallowances / totalltrs).toFixed(3);
            $('#daLtrs').text(daltrs);
            var talptrs = (taallowances / totalltrs).toFixed(3);
            $('#taPLtrs').text(talptrs);
        }
    }
    else {
        ShowMessage("'This Month has only '" + daysinmonth + "' Days'", "error")
        Modifieddayspresent = parseFloat(digit);
        var da = ((SetDailyAllowances) * parseFloat(digit));
        var setda = ((SetDailyAllowances) + "*" + parseFloat(digit))
        $('#DailyAllowance').text(da + "(" + setda + ")");
        $('#days').val(digit);
        var grossalry = Math.round((onedaysalary) * parseFloat(digit));
        $('#gsalary').text(grossalry);
    }
}
// After Salary Modification Salary Insert Function
function SaveSalary() {
    
    if (modified != "true") {
        ModifiedtotalSalary = totalsalary

        //ModifiedDailyAllowance = DailyAllowance;
    }
    SalespersonId
    var personname = $('#personname').text();
    var stateId = $('#stateId').val();
    daysPresent;
    var Modifieddayspresent = $('#days').val();
    var modifyDaysForDA = $('#daysforda').val();
    if (Modifieddayspresent == "") {
        Modifieddayspresent = 0;
    }
    var daily = $('#DailyAllowance').text();
    var ModifiedDailyAllowance = (parseInt(daily));
    Internet
    Mobile
    basicsalry
    totalsalary;
    var ModifiedtotalSalary = $('#totalsalary').text();
    var ta = $('#taallowance').text();
    var totalltrs = $('#totalltrs').text();
    var costPerLtrs = $('#costLtrs').text();
    var daPerLtrs = $('#daLtrs').text();
    var taPerLtrs = $('#taPLtrs').text();
    var incentive = $('#commission').val();
    var arrears = $('#arrears').val();
    var remarks = $('textarea#remark').val();
    var modifiedGrossSalary = $('#gsalary').text();
    grossSalary;
    if (incentive == "") {
        incentive = 0
    }
    else {
        incentive = incentive;
    }
    if (arrears == "") {
        arrears=0
    }
    else {
        arrears = arrears
    }
    
    $('#loader').show();
    $.ajax({
        url: '/Salary/UpdateSalary',
        data: { salesPersonId: SalespersonId, personName: personname, salaryDate: salarydate, daysPresent: daysPresent, modifiedDaysPresent: Modifieddayspresent, dailyAllowance: DailyAllowance, modifiedDailyAllowance: ModifiedDailyAllowance, internet: Internet, Mobile: Mobile, basicSalary: basicsalry, totalSalary: totalsalary, modifiedTotalSalary: ModifiedtotalSalary, ta: ta, totalLtrs: totalltrs, costPerLtrs: costPerLtrs, daPerLtrs: daPerLtrs, taPerLtrs: taPerLtrs, incentive: incentive, arrears: arrears, remarks: remarks, grossSalary: grossSalary, modifiedGrossSalary: modifiedGrossSalary, daysPresentForDA: daysPresent, modifyDaysForDA: modifyDaysForDA, stateId: stateId },
        success: function (data) {
            
            if (data == "done") {
                ShowMessage("Salary Updated Sucessfully!!!!!", "info")
                $('#loader').hide();
                $("#popup").dialog('close');
                Submit();
            }
        }
    });

}
// Send Email Salary PDF Format
function SendEmail(objnew, tableobj) {
    $('#loader').show();
    var tableobj = $(tableobj).parent().parent().parent();
    var ToEmailId = tableobj.find('#emailId').val();
    if (ToEmailId != "" && ToEmailId != null) {
        var FileN = tableobj.find('#fileName').val();
        var subject = tableobj.find('#subject').val();
        var body = tableobj.find('#body').val();
        var maindiv = $(objnew).parent().parent();
        var Email = maindiv.find('#email').html();
        var soname = maindiv.find('#soname').html();
        var ContactNo = maindiv.find('#contact').html();
        var days = maindiv.find('#dayspresent').html();
        var daysForDA = maindiv.find('#daysForDA').html();
        var dailyallowance = maindiv.find('#dailyallowance').html();
        var setallowances = maindiv.find('#setdailyallowances').html();
        var predefine = ((setallowances) + '*' + (daysForDA))
        var Internet = maindiv.find('#Internet').html();
        var mobile = maindiv.find('#mobile').html();
        var basicsalary = maindiv.find('#basicsalary').html();
        var totalsalary = maindiv.find('#totalsalry').html();


        var totalLtrs = maindiv.find('#TotalLtrs').html();
        var costPerLtrs = maindiv.find('#costPerLtrs').html();
        var daPerLtrs = maindiv.find('#daPerLtrs').html();
        var taPerLtrs = maindiv.find('#taPerLtrs').html();
        var incentive = maindiv.find('#incentive').html();
        var arrears = maindiv.find('#arrear').html();
        var remarks = maindiv.find('#remark').html();
        var ta = maindiv.find('#ta').html();
        var gross = maindiv.find('#grossSalary').html();
        var TargetLtrs = maindiv.find('#TargetLtrs').html();
        var ShopCount = maindiv.find('#NewShopCount').html();
        var designation = $('#personType').val();
        SalaryMonth;
        SalaryYear;
        $.ajax({
            url: '/Salary/SendEmail',
            data: { soname: soname, daysPresent: days, dailyAllowance: dailyallowance, internet: Internet, mobile: mobile, basicSalary: basicsalary, totalSalary: totalsalary, contactNo: ContactNo, salaryMonth: SalaryMonth, salaryYear: SalaryYear, toEmailId: ToEmailId, subject: subject, body: body, fileN: FileN, predefine: predefine, totalLtrs: totalLtrs, costPerLtrs: costPerLtrs, daPerLtrs: daPerLtrs, taPerLtrs: taPerLtrs, incentive: incentive, arrears: arrears, remarks: remarks, ta: ta, grossSalary: gross, TargetLtrs: TargetLtrs, NewShopCount: ShopCount, designation: designation },
            success: function (data) {
                
                if (data == "DONE") {
                    ShowMessage("Email Send Successfully!!!!!", "info")
                    $('#loader').fadeOut();
                    tableobj.find('#emailId').val('');
                    $("#emailPopupNew").dialog('close');
                }
            }
        });
    }
    else {
        ShowMessage("Please Enter Email Id !!!", "error");
        $('#loader').fadeOut();
    }
}
// Salary View In popup
function ClickView(obj) {
    $('#salaryslip').show();
    var maindiv = $(obj).parent().parent();
    var soname = maindiv.find('#soname').html();
    var ContactNo = maindiv.find('#contact').html();
    var Email = maindiv.find('#Email').html();
    var daysPresent = maindiv.find('#dayspresent').html();
    var grosssalary = maindiv.find('#grossSalary').html();
    var TotalLtrs = maindiv.find('#TotalLtrs').html();
    var costPerLtrs = maindiv.find('#costPerLtrs').html();
    var daPerLtrs = maindiv.find('#daPerLtrs').html();
    var ta = maindiv.find('#ta').html();
    var taPerLtrs = maindiv.find('#taPerLtrs').html();
    var TargetLtrs = maindiv.find('#TargetLtrs').html();
    var NewShopCount = maindiv.find('#NewShopCount').html();
    SalaryMonth
    SalaryYear
    var setallowance = maindiv.find('#setdailyallowances').html();
    var daysForDA = maindiv.find('#daysForDA').html();
    var test = ((setallowance) + '*' + (daysForDA))
    var dailyallowance = maindiv.find('#dailyallowance').html();
    var Internet = maindiv.find('#Internet').html();
    var mobile = maindiv.find('#mobile').html();
    var basicsalary = maindiv.find('#basicsalary').html();
    var totalsalary = maindiv.find('#totalsalry').html();
    var Incentive = maindiv.find('#incentive').html();
    var arrear = maindiv.find('#arrear').html();
    var remark = maindiv.find('#remark').html();
    var designation = $('#personType').val();
    
    $("#gridNew").empty();
    $("#salaryslip").empty();
    $("#salaryslip").append("<tbody><tr><td style='font-weight:bold;padding-left:15px; height:30px;'>Employee Name: &nbsp; &nbsp;" + soname + "</td><td style='font-weight:bold;padding-left:20px;'>Designation:&nbsp;&nbsp;" + designation + "</td></tr>" +
                           "<tr><td style='font-weight:bold;padding-left:15px; height:30px;'>Mobile No.: &nbsp; &nbsp;" + ContactNo + "</td><td style='font-weight:bold;padding-left:20px;'>Email Id:&nbsp;&nbsp;" + Email + "</td></tr>" +
                           "<tr><td style='font-weight:bold;padding-left:15px; height:30px;'>Salary Month: &nbsp; &nbsp;" + SalaryMonth + "</td><td style='font-weight:bold;padding-left:20px;'>Salary Year:&nbsp;&nbsp;" + SalaryYear + "</td></tr>" +
                           "<tr><td colspan='2'><table border='0' cellpadding='4' cellspacing='3' style='align-content:center; width:700px; height:300px'>" +
                           "<tr style='background-color:#1693EA;color:White'><th colspan='2' style='font-weight:bold; font-size:large; padding-left:15px; height:20px; border: 1px solid black;'>EARNINGS</th></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>Basic Salary(Rs.):<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + basicsalary + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>Days Present:<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + daysPresent + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>Gross Salary(Rs.):<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + grosssalary + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>D.A(Rs.):<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + (dailyallowance) + " " + "(" + "" + test + "" + ")" + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>Internet(Rs.):<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + Internet + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>Mobile(Rs.):<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + mobile + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>T.A.(Rs.):<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + ta + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>Incentive(Rs.):<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + Incentive + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>Arrear(Rs.):<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + arrear + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>Remark:<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + remark + "</td></tr>" +
                           "<tr style=background-color:gray;><td  style='font-weight:bold; font-size:large; padding-left:15px; height:20px; border: 1px solid black;'>Net Salary (Rs.):<td style='font-weight:bold; font-size:large; padding-left:15px; height:20px; border: 1px solid black;'>" + totalsalary + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>Target Ltrs.:<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + TargetLtrs + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>Total Ltrs. Achived:<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + TotalLtrs + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>Cost Per Ltrs.(Rs.):<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + costPerLtrs + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>D.A. Per Ltrs.(Rs.):<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + daPerLtrs + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>T.A. Per Ltrs.(Rs.):<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + taPerLtrs + "</td></tr>" +
                           "<tr><td  style='font-weight:bold;padding-left:15px; height:30px;border: 1px solid black ;'>New Shops Created:<td style='font-weight:bold;padding-left: 15px;border: 1px solid black ;'>" + NewShopCount + "</td></tr>" +
                           "<tr><td colspan='2' style='font-weight:bold;padding-left:15px; height:20px;border: 1px solid black ;'></td></tr>" +
                           "<tr> <td colspan='2' style='font-weight:bold;padding-left:35px; height:30px;'>This Salary Slip Computer Generated!!</td></tr>" +
                            "</table></td></tr></tbody>")

    OpenPopUp();
    $('#loader').fadeOut();
}
// Send Email in Excel Format
function SENDALLGrid(tableobj) {
    $('#loader').show();
    var tableobj = $(tableobj).parent().parent().parent();
    var ToEmailId = tableobj.find('#emailId').val();
    var subject = tableobj.find('#subject').val();
    var body = tableobj.find('#body').val();
    if (ToEmailId != "" && ToEmailId != null) {
        $.ajax({
            url: '/Salary/SENDALLGridINExcel',
            data: { toEmailId: ToEmailId },
            success: function (data) {
                
                if (data == "DONE") {
                    ShowMessage("Email Send Successfully!", "info")
                    $('#loader').fadeOut();
                    $("#emailAllPopup").dialog('close');
                }
                else {
                    ShowMessage("No salaries fixed yet!", "warning");
                    $('#loader').fadeOut();
                    $("#emailAllPopup").dialog('close');
                }
            }
        });
    }
    else {
        alert('Please Enter Email Id !!!!')
        $('#loader').fadeOut();
    }
}
function Totalcalc() {
    
    //alert(totalsumLtrs);
    var totalsumcostperltrs = 0;
    var totalsumdaperltrs = 0;
    var totalsumtaperltrs = 0;
    $('#gridNew').show();
    $('#salaryslip').hide();
    if (totalsumLtrs > 0) {
        totalsumcostperltrs = parseFloat((totalsumsalary) / (totalsumLtrs))
        totalsumdaperltrs = parseFloat((totalsumda) / (totalsumLtrs))
        totalsumtaperltrs = parseFloat((totalsumta) / (totalsumLtrs))
    }
    $("#gridNew").empty();
    $("#salaryslip").empty();
    $("#gridNew").append("<tr><td>Total Gross Salary (Rs.)</td><td>" + totalsumgrosssalary + "</td></tr><tr><td>Total D.A.(Rs.):</td><td>" + totalsumda + "</td></tr><tr><td>Total T.A. (Rs.):</td><td>" + totalsumta + "</td></tr>" +
                        "<tr><td>Total Internet (Rs.).</td><td>" + totalsuminternet + "</td></tr><tr><td>Total Mobile (Rs.):</td><td>" + totalsummobile + "</td></tr>" +
                        "<tr><td>Total Incentive (Rs.).</td><td>" + totalsumincentive + "</td></tr>" +
                        "<tr><td style='background-color:gray;'>Total Salary(Rs.):</td><td style='background-color:gray;'>" + totalsumsalary + "</td></tr>" +
                        "<tr><td>Target Ltrs.:</td><td>" + totaltargetltr + "</td></tr>" +
                        "<tr><td>Total Ltrs Achived:</td><td>" + totalsumLtrs + "</td></tr>" +
                         "<tr><td>Total New Shops:</td><td>" + totalsumNewShopcount + "</td></tr>" +
                        "<tr><td>Total Cost Per Ltrs(Rs.):</td><td>" + totalsumcostperltrs.toFixed(3) + "</td></tr><tr><td>Total D.A Per Ltrs (Rs.):</td><td>" + totalsumdaperltrs.toFixed(3) + "</td></tr>" +
                         "<tr><td>Total T.A. Per Ltrs. (Rs.).</td><td>" + totalsumtaperltrs.toFixed(3) + "</td></tr>");
    OpenPopUp();
    $('#loader').fadeOut();
}
function downloadexcel() {
    $.ajax({
        url: '/Salary/Download',
        data: {},
        success: function (data) {
            
            if (data == "NOTDONE") {
                ShowMessage("No salaries fixed yet!", "warning");
                $('#loader').fadeOut();
            } else {
                document.location = '/Salary/Download';
            }
        }
    });
}