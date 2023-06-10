<template>
	<view class="container">
		<view class="wrapper">
			<view class="title">
				用户注册
			</view>
			<view class="input-content">
				<u-form :model="form" ref="uForm">
					<u-form-item>
						<u-input type="id" placeholder="请输入账号" v-model="form.id" />
					</u-form-item>
					<u-form-item>
						<u-input type="password" placeholder="请输入密码" v-model="form.password" />
					</u-form-item>
					<u-form-item>
						<u-input type="nickname" placeholder="请输入昵称" v-model="form.nickname" />
					</u-form-item>
				</u-form>
			</view>
			<button class="confirm-btn" @tap="register()">注册</button>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				form: {
					id: '',
					password: '',
					nickname: '',
				},
		
			}
		},
		
		methods: {
		register() {
			uni.request({
			url: 'https://10.133.140.131:7053/api/todo/register',
			data: {
			id: this.form.id, // 从表单中获取账号
			password: this.form.password, // 从表单中获取密码
			nickname: this.form.nickname // 从表单中获取昵称
			},
			header: {
			'Content-Type': 'application/json' //设置请求头
				},
			method: 'POST',
			sslVerify:false,
			success: (res) => {
			if (res.statusCode == 200) {
			this.$u.toast('注册成功');
			uni.navigateBack();
			} else {
			this.$u.toast(res.data.message);
			}
			},
			fail: (err) => {
			console.log(err);
			}
			});
		}
		

			
			
		},
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
