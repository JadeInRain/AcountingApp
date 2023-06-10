<template>
	<view class="container">
		<fab></fab>
		<view class="user-section">
			<view class="bg"></view>
			<view class="user-info-box">
				<view class="portrait-box">
					<image class="portrait" :src="userInfo.avatar || '/static/missing-face.png'">
					</image>
				</view>
				<view class="info-box">
					<view v-if="hasLogin">
						<text class="username">{{userInfo.nickname || '游客'}}</text>
					</view>
					<view v-else>
						<text class="username" @click="goToLoginPage()">点击登录</text>
					</view>
				</view>
			</view>
		</view>
		<view class="tj-sction">
			<view class="tj-item">
				<text class="num">{{userData.sign_in_day || 0}}</text>
				<text>使用天数</text>
			</view>
			<view class="tj-item">
				<text class="num">{{userData.all_bill_day || 0}}</text>
				<text>总记账天数</text>
			</view>
			<view class="tj-item">
				<text class="num">{{userData.all_bill_count || 0}}</text>
				<text>总记账笔数</text>
			</view>
		</view>
		<view class="menu-container">
			<view class="menu-list">
				<u-cell-group>
					<u-cell-item @click="categorySet()" title="收支类型">
					</u-cell-item>
					<u-cell-item v-if="hasLogin" @click="logout()" title="注销登录">
					</u-cell-item>
				</u-cell-group>
			</view>
		</view>
		<u-select @click="select_bill_show = true" :default-value="default_bill_index" mode="single-column"
			v-model="select_bill_show" :list="bill_list" @confirm="billConfirm"></u-select>
	</view>
</template>

<script>
	import fab from '@/my-components/fab/index.vue'
	export default {
		components: {
			fab
		},
		data() {
			return {
				userData: {
					sign_in_day : 0,
					all_bill_day : 0,
					all_bill_count : 0,
					},
				phone_height: "0",
				scroll_view_height: "100",

				border_bottom: false,
				bill_title: "默认账单",

				select_bill_show: false,
				default_bill_index: [0],
				bill_list: [{
					value: '0',
					label: '默认账单',
					extra: '0'
				}],
				paths: {
					xtxx: '/pages/notification/index'
				}
			}
		},
		computed: {
			userInfo() {
				return this.$store.getters.user
			},
			unreadCount() {
				return this.$store.getters.unread_count || 0
			}
		},
		methods: {
			// #ifdef MP
			getCode,
			// #endif
			share() {
				console.log('非小程序分享')
			},
			// 点击选择账单
			selectBill() {
				this.select_bill_show = !this.select_bill_show;
			},
			// 选择账单回调
			billConfirm(e) {
				console.log(e);
			},

			message() {},
			pickerConfirm() {

			},
			logout() {
				this.$store.dispatch('logout')
			},
			wkf() {
				this.$u.toast('该功能暂未开放');
			},
			categorySet() {
				uni.navigateTo({
					url: "/pages/setting/category"
				})
			},
			goto(key, auth = true) {
				if (auth && !this.hasLogin) {
					this.$u.toast('请登录后查看');
					return
				}
				uni.navigateTo({
					url: this.paths[key]
				})
			}
		},
		onLoad(options) {},
		onShow() {
			if (this.hasLogin) {
				uni.request({
				    url: 'https://10.133.140.131:7053/api/todo/findbill',
				    method: 'POST',
					sslVerify:false,
				    success: (res) => {
						console.log("4")
						console.log(res.data)
				        this.userData.sign_in_day = res.data.usedDays;
						this.userData.all_bill_day = res.data.totalDays;
						this.userData.all_bill_count = res.data.totalCount;
				
				    },
				    fail: () => {
			
				    }
				});
			}
		}
	}
</script>

<style lang="scss">
	.container {
		background-color: #f5f5f5;
		height: 100vh;
	}

	%flex-center {
		display: flex;
		flex-direction: column;
		justify-content: center;
		align-items: center;
	}

	%section {
		display: flex;
		justify-content: space-around;
		align-content: center;
		background: #fff;
		border-radius: 10upx;
	}

	.user-section {
		padding: 100upx 30upx 0;
		position: relative;

		.bg {
			position: absolute;
			left: 0;
			top: 0;
			width: 100%;
			height: 100%;
			background-color: $uni-theme-color;
		}
	}

	.user-info-box {
		height: 180upx;
		display: flex;
		align-items: center;
		position: relative;
		z-index: 1;

		.portrait {
			width: 130upx;
			height: 130upx;
			border: 5upx solid #fff;
			border-radius: 50%;
		}

		.username {
			font-size: 30rpx;
			color: #fff;
			margin-left: 20rpx;

			&.qq_login_btn {
				background-color: $uni-theme-color;
			}
		}
	}

	.menu-container {
		background-color: #f5f5f5;
		position: relative;
		padding-bottom: 20rpx;
	}

	.tj-sction {
		@extend %section;

		background-color: #fff;

		.tj-item {
			@extend %flex-center;
			flex-direction: column;
			height: 140upx;
			font-size: $font-sm;
			color: $uni-text-color-grey;
		}

		.num {
			font-size: 40rpx;
			color: $uni-text-color;
			margin-bottom: 8upx;
		}
	}

	.menu-list {
		margin-top: 30upx;
		background: #fff;
		border-radius: 10upx;

		switch {
			transform: scale(0.7, 0.7)
		}
	}

	.list-item {
		width: 100%;
		padding: 0rpx;
		background-color: #FFFFFF !important;
		border: initial;
		text-align: initial;
	}

	.list-item::after {
		border: initial;
	}

	.select_bill {
		flex: 1;
		margin-right: 10px;
		text-align: right;
		color: #FFFFFF;
	}
</style>
