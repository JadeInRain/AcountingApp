<template>
	<view class="container">
		<view class="wrapper">
			<view class="title">
				用户登录
			</view>
			<view class="input-content">
				<u-form :model="form" ref="uForm">
					<u-form-item>
						<u-input placeholder="请输入用户名" v-model="form.id" />
					</u-form-item>
					<u-form-item>
						<u-input type="password" placeholder="请输入密码" v-model="form.password" />
					</u-form-item>
					
				</u-form>
			</view>
			<button class="confirm-btn" @tap="login()">登录</button>
			<button class="confirm-btn" @tap="toRegist()">注册</button>
		</view>
	</view>
</template>

<script>
import hasLogin from '../../mixins/hasLogin'
import { mapState } from 'vuex'


	export default {
		data() {
			return {
				form: {
					id: '',
					password: '',
					nickname:'',
				},
				
				mobile: '',
				password: '',
			}
		},
		computed: {
		  ...mapState({
		    user: state => state.user
		  })
		},
		methods: {
			toRegist() {
				uni.navigateTo({
					url: '/pages/login/register'
				})
				
			},
			
			    login() {
			        uni.request({
			            url: 'https://10.133.140.131:7053/api/todo/login',
			            data: {
			                id: this.form.id, // 从表单中获取账号
			                password: this.form.password // 从表单中获取密码
			            },
			            header: {
			                'Content-Type': 'application/json' //设置请求头
			            },
			            method: 'POST',
						sslVerify:false,
			            success: (res) => {
							console.log(res.statusCode);
							
			                if (res.statusCode == 200) {
								console.log("1");
			                    this.$u.toast('登录成功');
								console.log(res.data.user);
								uni.setStorageSync('UserInfo', res.data.user); //将用户信息存储到本地缓存中
								uni.setStorageSync('UserToken', res.data.user.token); //将用户token存储到本地缓存中
			                 	this.$store.commit('setToken', res.data.user.token); //存储用户token到Vuex
			                 	this.$store.commit('setUserInfo', res.data.user); //存储用户信息到Vuex
			                 	this.$store.dispatch('getUserInfo'); //调用Vuex中的Action方法
								
			                    uni.navigateBack();
			                } else {
								console.log("2");
			                    this.$u.toast(res.data.message);
			                }
			            },
			            fail: (err) => {
			                console.log(err);
			            }
			        });
			    }
			

		}
	}
</script>

<style lang='scss'>
	.container {
		padding-top: 50px;
		width: 100vw;
		height: 100vh;
		background: #fff;
	}

	.wrapper {
		background: #fff;
		padding-bottom: 40upx;
	}


	.title {
		text-align: center;
		margin-bottom: 100rpx;
		font-size: 46upx;
		color: #555;
		text-shadow: 1px 0px 1px rgba(0, 0, 0, .3);
	}

	.input-content {
		padding: 0 60upx;
	}

	.input-item {
		display: flex;
		flex-direction: column;
		align-items: flex-start;
		justify-content: center;
		padding: 0 30upx;
		background: $page-color-light;
		height: 120upx;
		border-radius: 4px;
		margin-bottom: 50upx;

		&:last-child {
			margin-bottom: 0;
		}

		.tit {
			height: 50upx;
			line-height: 56upx;
			font-size: $font-sm+2upx;
			color: $font-color-base;
		}

		input {
			height: 60upx;
			font-size: $font-base + 2upx;
			color: $font-color-dark;
			width: 100%;
		}
	}

	.confirm-btn {
		width: 630upx;
		height: 76upx;
		line-height: 76upx;
		border-radius: 50px;
		margin-top: 70upx;
		background: $uni-theme-color;
		color: #fff;
		font-size: $font-lg;

		&:after {
			border-radius: 100px;
		}
	}

	.forget-section {
		font-size: $font-sm+2upx;
		color: $uni-theme-color;
		text-align: center;
		margin-top: 40upx;
	}

	.register-section {
		position: absolute;
		left: 0;
		bottom: 50upx;
		width: 100%;
		font-size: $font-sm+2upx;
		color: $font-color-base;
		text-align: center;

		text {
			color: $uni-theme-color;
			margin-left: 10upx;
		}
	}

	.captcha_box {
		.input {}

		.captcha {
			width: 240rpx;
			height: 72rpx;
		}
	}
</style>
