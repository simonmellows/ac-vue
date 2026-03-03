import { ref, onMounted, onBeforeUnmount } from 'vue';

export default function useElementDimensions(elementRef) {
    const width = ref(0);
    const height = ref(0);
    const sizeX = ref('xs');
    const sizeY = ref(0);
    const wide = ref(false)
    const tall = ref(false)
    const hasWrapped = ref(false);
    

    const updateDimensions = () => {
        let element = elementRef.value;

        // Check if it's a Vue component instance and get the underlying DOM element
        if (element && element.$el) {
            element = element.$el;
        }

        if (element instanceof HTMLElement) {
            const rect = element.getBoundingClientRect();
            width.value = rect.width;
            height.value = rect.height;
            checkForWrap(element);

            // sizeX
            if(width.value < 300) sizeX.value = 'xs'
            else if(width.value < 360) sizeX.value = 'sm'
            else if(width.value < 470) sizeX.value = 'md'
            else if(width.value < 550) sizeX.value = 'lg'
            else if(width.value >= 550) sizeX.value = 'xl'
            // sizeY
            if(height.value < 300) sizeY.value = 0
            else if(height.value < 360) sizeY.value = 1
            else if(height.value < 470) sizeY.value = 2
            else if(height.value < 550) sizeY.value = 3
            else if(height.value >= 550) sizeY.value = 4
            // wide
            if(width.value > height.value){ 
                wide.value = true
                tall.value = false
            }
            // tall
            else {
                wide.value = false
                tall.value = true
            } 
        }
    };

    const checkForWrap = (element) => {
        const children = element.children;

        if (children.length > 1) {
            const firstChildTop = children[0].offsetTop;

            for (let i = 1; i < children.length; i++) {
                if (children[i].offsetTop !== firstChildTop) {
                    hasWrapped.value = true;
                    return;
                }
            }
        }

        hasWrapped.value = false;
    };

    const resizeObserver = new ResizeObserver(() => {
        updateDimensions()
    })

    //onMounted(() => {
        let element = elementRef.value;

        // Check if it's a Vue component instance and get the underlying DOM element
        if (element && element.$el) {
            element = element.$el;
        }
        updateDimensions();
        resizeObserver.observe(element)
    //});

    onBeforeUnmount(() => {
        resizeObserver.disconnect()
    });

    updateDimensions();

    return { width, height, sizeX, sizeY, hasWrapped, wide, tall };
}