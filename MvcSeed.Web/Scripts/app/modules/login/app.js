(function () {

    'use strict';

    var UserRoleEnum = {
        Student: 0,
        Employee: 1,
        Parent: 2
    };

    var questionnairesVM = new Vue({
        el: '#questionnaires',
        data: {
            items: [{
                Id: 0,
                Name: '',
                RecipientRole: 0
            }]
        },
        methods: {
            goSelectRecipient: function (item) {
                //如果选择的问卷时通用类型，直接跳到问卷详情页面
                if (item.RecipientRole === 2) {
                    window.location.href = "/WeiXin/Questionnaire/Detail?recipientId=" + 0 + "&questionnaireId=" + item.Id;
                } else {
                    window.location.href = "/WeiXin/Questionnaire/UserList?questionnaireId=" + item.Id;
                }
            }
        }
    });

    
})();
