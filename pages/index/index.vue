<template>
	<view class="container">
		<fab></fab>
		<view class="top">
			<view class="line">
				<view class="item">
					<view class="header-text"> {{year}} 年</view>
				</view>
				<view class="item">
					<view class="header-text">收入</view>
				</view>
				<view class="item">
					<view class="header-text">支出</view>
				</view>
			</view>
			<view class="line data">
				<view class="item">
					<view class="month-content">
						<view class="month-area" @click="clickDate">
							<view class="month font-2x">{{month}}</view>
							<view class="month_str">月</view>
							<u-icon class="icon-area" name="arrow-down-fill" color="#fff" size="30"></u-icon>
						</view>
						<view class="bar"></view>
					</view>
				</view>
				<view class="item">
					<view class="font-2x"> {{cashflow.in}} </view>
				</view>
				<view class="item">
					<view class="font-2x">{{ cashflow.out }} </view>
				</view>
			</view>
		</view>
		<view v-if="!hasLogin" class="empty">
			<u-empty text="未登录" mode="permission">
				<button size="mini" slot="bottom" class="go-to-login" @click="toLogin()">立即登录</button>
			</u-empty>
		</view>
		<template v-else>
			<view v-if="cashflow.list.length > 0" class="scroll">
				<scroll-view scroll-y="true" class="scroll-view" refresher-enabled="true"
					:refresher-triggered="refresherTriggered" @refresherrefresh="refresher()"
					@refresherrestore="refresherrestore()" @refresherabort="refresherabort()">
					<view>
						<view v-for="(item, index) in cashflow.list" :key="index" >
							<view class="u-flex list-box" @click="toDetail(item.id)">
								<view class="u-m-r-10 u-flex-1">
									<view class="header-text"> {{item.time}}</view>
								</view>
								<view class="u-m-r-10 u-flex-1">
									<view class="header-text">收入 {{item.inp}}</view>
								</view>
								<view class="u-m-r-10 u-flex-1">
									<view class="header-text">支出 {{item.outp}}</view>
								</view>
								
							</view>	
						</view>
					</view>
				</scroll-view>
			</view>
			<view v-else class="empty">
				<u-empty text="暂无明细" mode="list"></u-empty>
			</view>
		</template>

		<u-picker mode="time" v-model="picker_show" :params="picker_params" :default-time="picker_time"
			@confirm="pickerConfirm"></u-picker>
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
				picker_params: {
					year: true,
					month: true,
					day: false,
					hour: false,
					minute: false,
					second: false
				},
				picker_show: false,
				picker_time: "",
				cashflow: {
					in: "0.00",
					list: [],
					out: "0.00"
				},
				refresherTriggered: false,
				_refresherTriggered: false,
				year: "",
				month: ""
			}
		},
		methods: {
			clickDate() {
				this.picker_show = true;
			},
			cashflowReset() {
				this.cashflow = {
					in: "0.00",
					list: [],
					out: "0.00"
				}
			},
			pickerConfirm(e) {
				console.log(e)
				if (this.picker_params.year) this.year = e.year;
				if (this.picker_params.month) this.month = e.month;

				this.picker_time = this.year + "-" + this.month;
				this.getList()
			},
			refresher() {
				// console.log('下拉刷新');
				this.getList()
				if (this._refresherTriggered) {
					return;
				}
				this._refresherTriggered = true;
				if (!this.refresherTriggered) {
					this.refresherTriggered = true;
				}
			},
			refresherrestore() {
				// console.log("自定义下拉刷新被复位");
				this.refresherTriggered = false;
				this._refresherTriggered = false;
			},
			refresherabort() {
				// console.log("自定义下拉刷新被中止");
				this.refresherTriggered = false;
				this._refresherTriggered = false;
			},
			getList() {
				
			    if (!this.hasLogin) {
			        return;
			    }
			
			    uni.request({
			        url: 'https://10.133.140.131:7053/api/todo/getCashFlowList',
			        method: 'POST',
					sslVerify:false,
			        data: {
						year: parseInt(Number(this.year)),
						month: parseInt(Number(this.month)),
					},
					header: {
					    'Content-Type': 'application/json;charset=UTF-8'
					},
			        success: (res) => {
						
						
			            this.cashflow.list = res.data.data.list;
						this.cashflow.in = res.data.data.income;
						this.cashflow.out = res.data.data.outcome;
			            this.refresherrestore();
			        },
			        fail: () => {
					
			            this.refresherabort();
			        }
			    });
			},
			toDetail(id) {
				uni.navigateTo({
					url: `/pages/index/detail?id=`+id
				});
			},
			toLogin() {
				this.goToLoginPage()
			}
		},
		onShow() {
			this.getList()
			// console.log(this.$store.getters.cur_cashbook.id)
		},
		onLoad(options) {
			let t = new Date().toISOString().slice(0, 10);
			let t_a = t.split("-");
			this.year = t_a[0] || "";
			this.month = t_a[1] || "";
			this.picker_time = this.year + "-" + this.month;

			this.getList()
		}
	}
</script>

<style lang="scss">
	.container {
		background-color: #f7f7f7;

		// scroll-view 通过flex 布局 自适应
		height: 100vh;
		display: flex;
		flex-direction: column;

		.scroll,
		.empty {
			flex: 1;
			overflow: scroll;
		}

		.scroll-view {
			text-align: center;
			height: 100%;
		}

		// 自适应结束


		.header-icon {
			padding: 15rpx;
		}

		.list-box {
			padding: 18rpx 18rpx 18rpx 40rpx;
		}

		.list-box-children {
			display: -webkit-box;
			display: -webkit-flex;
			display: flex;
			-webkit-box-orient: horizontal;
			-webkit-box-direction: normal;
			-webkit-flex-direction: row;
			flex-direction: row;
			-webkit-box-align: center;
			-webkit-align-items: center;
			align-items: center;
			position: relative;
			box-sizing: border-box;
			width: 100%;
			padding: 26rpx 32rpx;
			font-size: 28rpx;
			line-height: 50rpx;
			color: #606266;
			background-color: #fff;
			text-align: left;

			.icon {
				font-size: 50rpx;
				padding-right: 10rpx;
			}

			.box-icon {
				width: 50rpx;
				height: 50rpx;
				margin-right: 35rpx;
			}

			.box-left {
				width: auto;
				font-weight: 500;
				font-size: 28rpx;
			}

			.box-right {
				overflow: hidden;
				text-align: right;
				vertical-align: middle;
				color: #909399;
				font-size: 26rpx;
			}

			.box-remark {
				font-weight: 500;
				width: 300rpx;
				margin-left: 50rpx;
				overflow: hidden;
				text-overflow: ellipsis;
				-ms-text-overflow: ellipsis;
				display: -webkit-box;
				line-clamp: 1;
				-webkit-line-clamp: 1;
				-webkit-box-orient: vertical;
			}
		}
	}

	.go-to-login {
		border-radius: 10rpx;
		background: $uni-theme-color;
		color: #fff;
	}

	.top {
		.line {
			display: flex;
			color: #fff;
			background-color: $uni-theme-color;
			padding-left: 30rpx;
			padding-right: 30rpx;
			padding-bottom: 30rpx;

			&.data {
				padding-right: 20rpx;

				.item {
					flex: 1;
				}

				.font-2x {
					font-size: 40rpx;
				}

			}

			.item {
				flex: 1;
			}

			.header-text {
				font-size: 25rpx;
			}

			.bar {
				margin: 0;
				padding-left: 50rpx;
				border-right-width: 1px;
				border-right-style: solid;
				height: 50rpx;
				transform: scaleX(0.5);
				border-color: #fff;
			}

			.month-content {
				display: flex;

				.month-area {
					display: flex;
					// align-items: flex-end;
					align-items: center;
				}
			}
		}
	}
</style>