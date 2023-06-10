<template>
	<view class="charts-box">
		<qiun-data-charts type="column" :chartData="chartData"  />
	</view>
</template>

<script>
	//收支柱状图
	export default {
		props: {
			list: {
				type: Object 
			}
		},
		data() {
			return {
				opts: {
					"legend": {
						"show": true,
						"position": "bottom",
					},
					"extra": {
						"column": {
							"activeOpacity": 1,
							"border": false,
						},
					}
				},
				//  chartData: {
				// 	"categories": [
				// 		"2016",
				// 		"2017",
				// 		"2018",
				// 		"2019",
				// 		"2020",
				// 		"2021",
				// 		"2022",
				// 		"2023",
				// 		"2024",
				// 	],
				// 	"series": [{
				// 			"name": "目标值",
				// 			"data": [
				// 				35,
				// 				36,
				// 				31,
				// 				33,
				// 				13,
				// 				34,
				// 				34, 34, 34,
				// 			]
				// 		},
				// 		{
				// 			"name": "完成量",
				// 			"data": [
				// 				18,
				// 				27,
				// 				21,
				// 				24,
				// 				6,
				// 				28, 34, 34, 34,
				// 			]
				// 		}
				// 	]
				// }
			}
		},
		computed: {
			chartData() {
				return this.list
			}
		},
		
		methods: {
			complete(e) {
				// console.log(e)
			}
		}
	}
</script>

<style>
	.charts-box {
		width: 100%;
		height: 300px;
	}
</style>
