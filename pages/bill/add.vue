<template>
	<BillEditer @submit="submit"></BillEditer>
</template>

<script>
	import BillEditer from '@/my-components/bill-editer/index.vue'
	export default {
		components: {
			BillEditer,
		},
		data() {
			return {

			}
		},
		methods: {
			submit(data) {
					console.log(data)
					console.log(this.$store.getters.user)
					console.log(this.$store.getters.user_token)
					console.log(this.$store.getters.cur_cashbook)
				uni.request({
					url: 'https://10.133.140.131:7053/api/todo/createCashFlow',
					data: data,
					sslVerify:false,
					method: 'POST',
					header: {
						'content-type': 'application/json'
					},
					success: (res) => {
					console.log(res)
					if (res.data.code == 0) {
						uni.switchTab({
						url: '/pages/index/index',
						
					})
					} else {
						this.$u.toast(res.data.message);
					}
					},
					fail: (err) => {
						console.log(err)
					}
					})
				}
		},
	}
</script>

<style lang="scss">

</style>
