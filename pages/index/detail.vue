<template>
	<view class="container">
		<view class="info_box">
			<view class="line">
				<view style="font-size: 50rpx;
				min-height: 100rpx; 
				height: 100rpx;
				width: 50%;">
					{{info.amount}}
				</view>
			</view>

			<u-line></u-line>

			<view class="line">
				<text class="type">日期</text>
				<view style="font-size: 28rpx;font-weight: 600;">
					{{info.date}}
				</view>
			</view>
			<view class="line">
				<text class="type">分类</text>
				<view style="font-size: 25rpx;font-weight: 600;">
					{{info.categoryname}}
				</view>
			</view>

			<view class="line">
				<text class="type">备注</text>
				<view style="font-size: 28rpx;width: 70%;">
					{{info.remark || ''}}
				</view>
			</view>
			<view style="display: flex; padding: 20rpx;">
				<view style="margin: auto;">
					<u-button type="success" size="medium" @click="toEdit()">编辑</u-button>
				</view>
				<view style="margin: auto;">
					<u-button type="" size="medium" @click="delCashflow()">删除</u-button>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				info: {
					amount:"200",
					categoryname: "教育",
					id: null,
					name: "教育",
					date: "2021-05-12",
					image: null,
					remark: null,
				},
				id: null
			}
		},
		methods: {
					
					getCashflowInfo() {
								uni.request({
								
								    url: 'https://10.133.140.131:7053/api/todo/getCashFlowInfo',
								    method: 'POST',
									sslVerify:false,
										
								    data:{
										cashFlowId:this.id,
									},
								    success: (res) => {
									const cashFlowList = res.data.data.list;
									const cashFlow = cashFlowList.find((item) => item.id === this.id);
									this.info.date = cashFlow.time;
									this.info.amount = cashFlow.amount;
									this.info.categoryname = cashFlow.category_id;
									this.info.remark=cashFlow.remark;
								    },
								    fail: () => {
								       
								    }
								});
								},

					toEdit() {
						let id = this.id
						uni.navigateTo({
							url: `/pages/index/edit?id=`+id
						});
					},
					
					delCashflow() {
					  uni.request({
					    url: 'https://10.133.140.131:7053/api/todo/delCashFlow',
					    method: 'POST',
						sslVerify:false,
					    data: {
							cashFlowIdDel: this.id
					    },
					    success: (res) => {
								console.log(this.id)
						        console.log(res.data)
								
						        if (res.data.code === 0) {
						          // 删除成功
						          delete this.info.id;
						          this.$u.toast('删除成功'); 
						          uni.navigateBack();
						        } else {
						          // 删除失败
						          console.log("删除失败");
						        }
					    },
					    fail: (err) => {
					      console.error(err);
					    },
						
					  });
					}
				},

		onShow() {
			this.getCashflowInfo()
		},
		onLoad(options) {
			if (options.id) {
				this.id = options.id
			}
		}
	}
</script>

<style lang="scss" scoped>
	.info_box {
		padding: 30rpx;

		.del-btn {
			position: absolute;
			top: 5rpx;
			left: 645rpx;
			z-index: 10;

			border-radius: 100rpx;
			width: 44rpx;
			height: 44rpx;
			display: -webkit-box;
			display: -webkit-flex;
			display: flex;
			-webkit-box-align: center;
			-webkit-align-items: center;
			align-items: center;
			-webkit-box-pack: center;
			-webkit-justify-content: center;
			justify-content: center;

		}
	}

	.line {
		display: flex;
		align-items: center;
		width: 100%;
		padding: 26rpx 32rpx;
		font-size: 28rpx;
		line-height: 54rpx;
		color: #606266;
		background-color: #fff;
	}

	.type {
		font-size: 28rpx;
		padding-right: 30rpx;
	}

	.photo_icon {
		width: 100rpx;
		height: 100rpx;
		margin-left: auto;
	}
</style>
