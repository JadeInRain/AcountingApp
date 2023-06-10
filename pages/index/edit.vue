<template>
	<BillEditer v-if="id!=null" :value="id" @submit="submit"></BillEditer>
</template>

<script>
	import BillEditer from '@/my-components/bill-editer/index.vue'
	export default {
		components: {
			BillEditer,
		},
		data() {
			return {
				id: null
			}
		},
		methods: {
		    submit(Data) {
		      // 根据您的需求进行编辑数据的处理
		      // 将 cashFlowId 和 updatedData 发送到后端进行编辑操作
			  console.log(this.id);
			  Data.id=this.id;
			  console.log(Data);
		      uni.request({
		        url: 'https://10.133.140.131:7053/api/todo/editCashFlow',
		        method: 'POST',
				sslVerify:false,
		        data: {
					updatedData:Data,
		          cashFlowId:this.id
		          
		        },
				
		        success: (res) => {
		          const { code, message } = res.data
				  console.log(res.data);
		          if (code === 0) {
		            // 编辑成功
		            this.$u.toast('编辑成功')
		            uni.navigateBack()
		          } else {
		            // 编辑失败
		            console.log('编辑失败')
		          }
		        },
		        fail: (err) => {
		          console.error(err)
		        },
		      })
		    },
		  },
		onLoad(options) {
			if (options.id) {
				this.id = options.id
			}
		},
	}
</script>

<style lang="scss">

</style>
