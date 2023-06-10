<template>
	<view class="container">
		<view class="type_selector">
			<u-subsection class="selector" :animation="false" :list="list" @change="typeChange" active-color="#fff"
				:current="formData.type" mode="subsection">
			</u-subsection>
		</view>
		<view class="category-list">
			<view class="in-list" v-show="formData.type == 0"> 
						<CategoryList @openSet="openSet" @change="categoryChnage" :value="formData.categoryId"
							:list="out_list">
						</CategoryList>  
			</view>
			<view class="out-list" v-show="formData.type == 1"> 
						<CategoryList @openSet="openSet" @change="categoryChnage" :value="formData.categoryId"
							:list="in_list">
						</CategoryList> 
			</view>
		</view>

		<view class="input_box">
			<view class="line">
				<input type="digit" :value="formData.amount" class="amount_input" placeholder="0.00" :focus="true"
					@input="onInput" @confirm="submit()" />
			</view>

			<u-line></u-line>

			<view class="line">
				<view class="date">
					<text class="popup_type">日期</text>
					<view style="font-size: 28rpx;font-weight: 600;" @click="picker_show = true">
						{{formData.date}}
						<u-icon name="arrow-right" class="u-icon-wrap u-cell__right-icon-wrap"></u-icon>
					</view>
				</view>

				<u-button class="save_btn" type="success" size="medium" @tap="submit()">保存</u-button>
			</view>
			<view class="line">
				<text class="popup_type">备注</text>
				<input type="text" :value="formData.remark" @input="onRemarkInput" placeholder="添加备注"
					style="font-size: 28rpx;width: 70%;" />
			</view>
		</view>

		<u-picker mode="time" v-model="picker_show" :params="pickerOption" :default-time="formData.date"
			@confirm="pickerConfirm"></u-picker>
	</view>
</template>

<script>
	import dayjs from '@/dayjs.min.js'
	import CategoryList from '@/my-components/category-list/index.vue'
	import _ from 'lodash'
	export default {
		components: {
			CategoryList
		},
		props: {
			value: {
				type: Number,
				default: null
			}
		},
		data() {
			return {
				list: ['支出', '收入'],
				formData: {
					id: "123",
					type: 0,
					amount: null,
					categoryId: null,
					date: '',
					remark: '',
					image: null
				},
				pickerOption: {
					year: true,
					month: true,
					day: true,
					hour: false,
					minute: false,
					second: false
				},
				picker_show: false,

				desc: "",
				cur_cotegry_name: "",

				out_list: [
					{
						id:1,
						name:"交通",
						icon:"/static/car.png"
					},
					{
						id:2,
						name:"住房",
						icon:"/static/house.png"
					},
					{
						id:3,
						name:"餐饮",
						icon:"/static/eat.png"
					},
					{
						id:4,
						name:"日用",
						icon:"/static/sun.png"
					},
					{
						id:5,
						name:"其他",
						icon:"/static/other.png"
					}
				],
				in_list: [
					{
						id:1,
						name:"工资",
						icon:"/static/getmoney.png"
					},
					{
						id:2,
						name:"转账",
						icon:"/static/changemoney.png"
					},
					{
						id:3,
						name:"理财",
						icon:"/static/outmoney.png"
					},
					{
						id:4,
						name:"兼职",
						icon:"/static/othermoney.png"
					},
					{
						id:5,
						name:"其他",
						icon:"/static/other.png"
					}
				],

				empty_img: '../../static/empty-img.png',
				img: '',

				iconclear: {
					color: "red",
					size: '20',
					type: 'clear'
				},
			}
		},
		methods: {
			typeChange(index) {
				this.formData.type = index;
				this.formData.categoryId = null
			}, 
			openSet() {
				console.log('打开设置页面')
				uni.navigateTo({
					url: "/pages/setting/category"
				})
			},
			listSet(list = []) {
				list.push({
					icon: "red-packet",
					name: "设置",
					is_set: true,
					id: 0
				})
				let arr = []
				let tmp = []
				for (let i = 0; i < list.length; i++) {
					if (i != 0 && (i % 10 == 0)) {
						arr.push(tmp)
						tmp = []
					} else {
						tmp.push(list[i])
					}
				}
				arr.push(tmp)
				tmp = []
				return arr
			},
			categoryChnage(id) {
				if(this.formData.type===0){
					this.formData.categoryId=this.out_list.find(x=>x.id==id).name
				}else{
					this.formData.categoryId=this.in_list.find(x=>x.id==id).name
				} 
			},
			pickerConfirm(e) {
				this.formData.date = e.year + '-' + e.month + "-" + e.day;
			},
			onInput(e) {
				let v = e.detail.value;
				v = v.replace(/^\D*([1-9]\d{0,6}\.?\d{0,2})?.*$/, '$1');
				this.formData.amount = v;
				return v;
			},
			onRemarkInput(e) {
				this.formData.remark = e.detail.value;
			},
			submit() {
				let data = {
					id: this.formData.id,
					type: this.formData.type == 0 ? 20 : 10, // 20支持 10 收入
					categoryId: this.formData.categoryId,
					amount: this.formData.amount,
					date: this.formData.date,
					image: null,
					remark: this.formData.remark
				}
				this.$emit('submit', data)
				
			},
			getCashflowInfo(id) {
				this.$u.api.getCashflowInfo(id).then(res => {
					let data = res.data
					console.log(data.type)
					this.formData = {
						id: data.id,
						type: data.type == 20 ? 0 : 1,
						amount: data.amount,
						categoryId: data.categoryId,
						date: data.date,
						remark: data.remark,
						image: data.image,
					}
				})
			},
		},
		created() {
			//this.getCategory()
			this.formData.date = dayjs().format('YYYY-MM-DD')
			if (this.value !== null) {
				this.getCashflowInfo(this.value)
			}
		}
	}
</script>

<style lang="scss">
	.container {
		background-color: #ffffff;

		.type_selector {
			width: 100%;
			background-color: $uni-theme-color;
			display: flex;
			justify-content: center;
			padding-bottom: 30rpx;

			.selector {
				width: 80%;
			}
		}
	}

	.input_box {
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
		padding: 26rpx 32rpx;
		font-size: 28rpx;
		line-height: 54rpx;
		color: #606266;
		background-color: #fff;

		.date {
			flex: 1;
			display: flex;
		}

		.save_btn {
			text-align: right;
		}

		.amount_input {
			font-size: 50rpx;
			min-height: 100rpx;
			height: 100rpx;
			width: 100%;
		}
	}


	.popup_type {
		font-size: 28rpx;
		padding-right: 30rpx;
	}

	.u-cell__right-icon-wrap {
		display: inline-flex;
		align-items: center;
		padding-left: 10rpx;
	}

	.photo_icon {
		width: 100rpx;
		height: 100rpx;
		margin-left: auto;
	}

	.category-list {
		padding-top: 50rpx;

		.swiper {
			width: 100%;
		}
	}
</style>
