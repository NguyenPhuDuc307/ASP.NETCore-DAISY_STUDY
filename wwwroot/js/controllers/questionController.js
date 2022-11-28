var questionController = {
    init: function () {
        questionController.registerEvent();
        questionController.loadData();
    },
    registerEvent: function () {
        
    },
    loadData: function () {
        $.ajax({
            url: '/Question/LoadData',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ExamScheduleID: item.examScheduleID,
                            QuestionString: item.questionString, 
                            Point: item.point,
                            ImagePath: item.imagePath
                        });
                    });
                    $('#tbData').html(html)
                }
            }
        })
    }
}
questionController.init();