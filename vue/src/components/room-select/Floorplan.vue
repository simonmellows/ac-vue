<template>
    <div class="d-flex flex-column h-100 w-100">
        <div v-if="!ready" class="w-100 h-100 d-flex">
            <v-icon icon="mdi-loading" style="font-size: 100px;" class="spinner my-auto mx-auto"></v-icon>
        </div>
        <!-- svg -->
        <div class="d-flex h-100 w-100">
            <!--<div>
                <div class="d-flex flex-column overflow-auto ga-2 no-scrollbar">
                    <v-btn prepend-icon="mdi-lightbulb" rounded="xl">
                        Lighting
                    </v-btn>
                    <v-btn prepend-icon="mdi-roller-shade" rounded="xl">
                        Shading
                    </v-btn>
                    <v-btn prepend-icon="mdi-thermometer" rounded="xl">
                        Climate
                    </v-btn>
                </div>
            </div>-->
            <div class="h-100 w-100" id="roomMap" v-show="ready">
                <div style="touch-action: none;" id="svg-container" ref="svgContainer" class="h-100 w-100 ">
                </div>
            </div>
        </div>
    </div>
</template>

<script setup>
import { ref, watch, onMounted, onBeforeUnmount, computed } from 'vue';
import Panzoom from '@panzoom/panzoom'
import useRooms from '../../composables/useRooms'
import colors from 'vuetify/util/colors'

const props = defineProps(['floorplan']);
const emit = defineEmits(['roomSelected']);
const svgContainer = ref(null);
const ready = ref(false)
const panzoom = ref(null)

const { getRoomByIndex, rooms } = useRooms()

const roomLabelSize = 10

function createSvg(data) {
    const parser = new DOMParser();
    const svgDocument = parser.parseFromString(data, "image/svg+xml");
    const svgElement = svgDocument.documentElement;    
    svgElement.setAttribute('width', '100%')
    svgElement.setAttribute('height', '100%')
    svgElement.style.padding = `${props.padding}em`
    return svgElement;
}

function updateDOMElement(newData) {
    if (svgContainer.value) {
        // Remove the previous element
        while (svgContainer.value.firstChild) {
            svgContainer.value.removeChild(svgContainer.value.firstChild);
        }
        // Create and append the new element
        const newElement = createSvg(newData);
        svgContainer.value.appendChild(newElement);
        createMap();
        ready.value = true
        
    }
}

// Update the DOM
watch(() => props.floorplan, (floorplan) => {
    if (floorplan) {
        updateDOMElement(floorplan);
    }
});

onMounted(() => {
    if (props.floorplan) {
        updateDOMElement(props.floorplan);
    }
})

// Initialize the floorplan
function createMap(){
    // Constant that references the SVG element
    const svg = svgContainer.value?.querySelector('svg')

    // Declare panzoom constant to utilise zoom and pan functionality
    panzoom.value = Panzoom(svg, {
        // Set initial values 
        maxScale: 1,
        contain: 'outside',
        step: 0.5,
        startScale: 1
    })

    const roomDataElements = svg.querySelectorAll('[roomData=true][roomId]');
    const roomAreaElements = svg.querySelectorAll('[roomArea=true][roomId]');
    console.log("Room data elements: ", roomDataElements)

    // Operations for room data elements
    if(roomDataElements.length){   
        roomDataElements.forEach(roomDataElement => {
            if(roomDataElement.getAttribute('x') && roomDataElement.getAttribute('y'))
            {
                const container = document.createElementNS("http://www.w3.org/2000/svg", "foreignObject")
                const roomId = roomDataElement.getAttribute('roomId')
                const roomIndex = parseInt(roomId)
                const textScale = parseFloat(roomDataElement.getAttribute('textScale') || 1)
                
                // Initialize room data elements to remove outline and add transparent fill
                roomDataElement.style.strokeWidth = 0;
                roomDataElement.style.fill = 'grey';
                roomDataElement.style.fillOpacity = 0;

                // Create container
                container.setAttribute('x', roomDataElement.getAttribute('x'))
                container.setAttribute('y', roomDataElement.getAttribute('y'))
                container.setAttribute('width', roomDataElement.getAttribute('width'))
                container.setAttribute('height', roomDataElement.getAttribute('height'))
                container.setAttribute('roomSelect', roomId)
                container.style.letterSpacing = "0.6px"
                container.style.fontSize = `${(roomLabelSize * textScale) / (panzoom.value.getScale())}px`

                // Create a div for text area
                var outerDiv = document.createElement("div");
                outerDiv.style.width = "100%";
                outerDiv.style.height = "100%";
                outerDiv.style.textAlign = "center";
                //outerDiv.style.wordWrap = "break-word"; // Enable text wrapping
                outerDiv.classList.add("d-flex", "flex-column")
                outerDiv.setAttribute('roomSelect', roomId)

                // Inner div
                var innerDiv = document.createElement("div");
                innerDiv.style.marginTop = "auto"
                innerDiv.style.marginBottom = "auto"
                innerDiv.setAttribute("roomSelect", roomId)
                innerDiv.classList.add("text-truncate")
                innerDiv.id = `${roomId}-floorplanFeedbackRoom`
                innerDiv.setAttribute('textScale', textScale)

                // Room label
                var roomLabel = document.createElement("span");
                console.log("Area list: ", props.areaList)
                // Add code here to set correct room name...
                roomLabel.innerHTML = `${getRoomByIndex(roomIndex)?.label || `room${roomId}`}<br>`;
                //roomLabel.style.fontSize = `${(roomLabelSize * textScale) / (panzoom.value.getScale())}px`
                roomLabel.setAttribute('roomSelect', roomId)
                roomLabel.setAttribute('id', `${roomId}-roomLabelSpan`)
                roomLabel.setAttribute('textScale', textScale)
                //roomLabel.style.backgroundColor = 'rgba(0,0,0,1)'

                // Append created elements
                innerDiv.appendChild(roomLabel)
                outerDiv.appendChild(innerDiv);
                container.appendChild(outerDiv);
                svg.appendChild(container)    
            }
        });
    }

    // Operations for room area elements
    if(roomAreaElements.length){   
        roomAreaElements.forEach(roomDataElement => {
            let roomId = roomDataElement.getAttribute('roomId')
            let roomIndex = parseInt(roomId)

            // Initialize room data elements to remove outline and add transparent fill
            roomDataElement.style.strokeWidth = 0;
            roomDataElement.style.fill = 'grey';
            roomDataElement.style.fillOpacity = 0;
            // Allow these elements to be selectable
            roomDataElement.setAttribute('roomSelect', roomId)

            // Add code here for setting fills based on lighting etc.
            //roomDataElement.style.fill = colors?.amber?.base || 'yellow';
            //roomDataElement.style.fillOpacity = 0.4;
        })
    }

    /* Event listeners */
    // Add event listener to listen out for scroll events (for zooming)
    const wheelEventListener = function (e) {
       panzoom.value.zoomWithWheel(e)
    }
    // Elements pressed
    svg.onclick = function (e) {
        let roomId = e.target.getAttribute('roomSelect')
        let roomIndex = parseInt(roomId)
        if (roomId) {
            //let id = parseInt(e.target.getAttribute('roomArea') || e.target.getAttribute('roomData') || e.target.getAttribute('roomSelect'))
            console.log(`Room ${roomId} selected`)
            // Room selected...
            emit('roomSelected', roomId)
        }
    }
    // Add event listener for when svg is zooming
    const zoomEventListener = function (e) {
        // Scale content here...
        // Scale room labels
        svg.querySelectorAll("[roomSelect]").forEach(element => {
            const textScale = parseFloat(element.getAttribute('textScale') || 1)
            element.style.fontSize = `${(roomLabelSize * textScale) / (panzoom.value.getScale())}px`
            element.style.letterSpacing = `${0.6 / (panzoom.value.getScale())}px`
        })
    }

    // Add event listeners
    //svg.addEventListener('wheel', wheelEventListener);
    //svg.addEventListener('panzoomzoom', zoomEventListener);

    
}

</script>

<style scoped>

</style>