

function ConfirmDelete(item) {
    
    return confirm("Are you sure you want to delete/remove this " + item);
    
}

function RemoveFromMyAgenda(ProgramId)
{
    
    jQuery.ajax({
        type: "POST",
        url: "/Home/RemoveFromMyAgenda",
        //contentType: "application/json; charset=utf-8",
        data: { ProgramId: ProgramId },
        dataType: "json",
        success: function (data) {

            if(data == true)
            {
                jQuery('#RemoveFromMyAgenda-' + ProgramId).parent().parent().parent().parent().remove();
            }
            //LoadConversationReplies(data);
            //jQuery('.Hide').hide();
            
        },
        error: function (xhr, error) {
            console.log(xhr); console.log(error);
        }
    });
}

function AddToMyAgenda(element, ProgramId) {
    
    jQuery.ajax({
        type: "POST",
        url: "/Home/AddToMyAgenda",
        //contentType: "application/json; charset=utf-8",
        data: { ProgramId: ProgramId },
        dataType: "json",
        success: function (data) {

            if (data == true) {
                jQuery(element).remove();
            }

        },
        error: function (xhr, error) {
            console.log(xhr); console.log(error);
        }
    });
}



function getMenu(Parameter) {

    var menujson = '{"result":{"array":['

    jQuery('#MobileMenu li').each(function (index) {

        if (index != 5 && index != 9 && index != 10 && index != 12 && index != 13) {
            if (index == 0) {
                menujson += '{"name": "' + jQuery(this).find('a').text() + '", ' + '"function":"javascript:GotoUrl(\'' + jQuery(this).find('a').attr('href') + '\');"}';
            }
            else {
                menujson += ',{"name": "' + jQuery(this).find('a').text() + '", ' + '"function":"javascript:GotoUrl(\'' + jQuery(this).find('a').attr('href') + '\');"}';
            }
        }
        else {
            menujson += ',{"name": "' + jQuery(this).find('a').text() + '", ' + '"function":"javascript:alert(\'Feature Not Available\');"}';
        }

    });
    menujson += ']}}';

    Parameter.json= menujson ;
    
    
}