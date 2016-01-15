require('script!../../../libs/rsa/BigInt.js');
require('script!../../../libs/rsa/RSA.js');
require('script!../../../libs/rsa/Barrett.js');

var Vue = require('vue');

require('vue-resource');

/*
  RSA 加密
 */
function pwdEncrypt(password) {
    var keyExponent = document.getElementById('keyExponent').value;
    var keyModulus = document.getElementById('keyModulus').value;

    setMaxDigits(130);
    var key = new RSAKeyPair(keyExponent, "", keyModulus);
    return encryptedString(key, password);
}

module.exports = {
    template: require('./template.html'),
    replace: true,
    data: function() {
        return {
            username: '',
            password: '',
            pwdRSA: '',
            exponent: '',
            modulus: ''
        }
    },
    methods: {
        login: function() {
            this.pwdRSA = pwdEncrypt(this.password);

            Vue.http.post("/Home/LoginDo", {
                UserName: this.username,
                Password: this.pwdRSA
            }, function(result) {
                if (result.State === 1) {
                    window.location.href = "/Home/Index";
                } else {
                    alert('账号密码错误');
                }
            });
        },
        authorizeGithub: function() {
            window.location.href = "https://github.com/login/oauth/authorize?client_id=b418a0c0efe60e558b64&scope=user,gist&state=mvcseed";
        }
    },
    // document.ready()
    ready: function() {
        Vue.http.post("/Home/GetRSAPublicKey", {}).then(function(result) {
            console.log(result.data);
            
            document.getElementById('keyExponent').value = result.data.Exponent;
            document.getElementById('keyModulus').value = result.data.Modulus;
        });
    }
}
