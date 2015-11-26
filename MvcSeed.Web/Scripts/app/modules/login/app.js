(function () {

    'use strict';

    var vm = new Vue({
        el: '#vm-login',
        data: {
            username: '',
            password: '',
            pwdRSA: ''
        },
        methods: {
            login: function () {
                pwdEncrypt();

                Vue.http.post("/Home/LoginDo", {
                    UserName: this.username,
                    Password: this.pwdRSA
                }, function (result) {
                    if (result.State === 1) {
                        window.location.href = "/Home/Index";
                    } else {
                        alert('账号密码错误');
                    }
                });
            },
            authorizeGithub: function () {
                window.location.href = "https://github.com/login/oauth/authorize?client_id=b418a0c0efe60e558b64&scope=user,gist&state=mvcseed";
            }
        }
    });

    function pwdEncrypt() {
        var keyExponent = document.getElementById('keyExponent').value;
        var keyModulus = document.getElementById('keyModulus').value;

        setMaxDigits(130);
        var key = new RSAKeyPair(keyExponent, "", keyModulus);
        vm.pwdRSA = encryptedString(key, vm.password);
    }

})();
