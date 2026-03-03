module.exports = {
    publicPath: '',
    chainWebpack: config => {
      // TypeScript configuration
      config.module
        .rule('typescript')
        .test(/\.tsx?$/)
        .use('ts-loader')
        .loader('ts-loader')
        .options({
          transpileOnly: true, // Speed up compilation
          appendTsSuffixTo: [/\.vue$/] // Apply TypeScript processing to .vue files
        })
        .end();
  
      // Optional: If you have custom SVG or other file handling needs, add those rules here
    }
  }