(ns aoc-2023.day-10
  (:require [aoc-2023.helpers :as helpers]
            [clojure.set :as set]
            [clojure.string :as string]))

(def input-file-path "inputs/day_10/input")

(defn get-start
  [field]
  (let [x (first (filter #(not (= -1 %)) (map #(.indexOf % \S) field)))
        y (.indexOf (apply vector (map #(.indexOf % \S) field)) x)]
    {:x x :y y}))

(defn parse-input
  [filename]
  (let [entries (string/split (slurp filename) #"\n")]
    (map #(apply vector %) entries)))

(defn field-get
  [field pos]
  (let [x (get pos :x)
        y (get pos :y)]
    (if (and (< y (count field)) (< x (count (first field)))
             (>= y 0) (>= x 0))
      (nth (nth field (get pos :y)) (get pos :x))
      nil)))

(defn get-potential-dirs
  [field pos]
  (let [north (update pos :y dec)
        south (update pos :y inc)
        west (update pos :x dec)
        east (update pos :x inc)]
    (filter #(not (nil? %))
            (vector
             (and (some #{(field-get field north)} [\| \7 \F]) north)
             (and (some #{(field-get field south)} [\| \L \J]) south)
             (and (some #{(field-get field west)} [\- \L \F]) west)
             (and (some #{(field-get field east)} [\- \J \7]) east)))))

(defn check-face-north
  [field pos]
  (let [north (update pos :y inc)]
    (some #{field-get field north} [\| \7 \F])))

(defn get-next                                                                                                                                                             [field pos dx dy]                                                                                                                                                        (let [dir (field-get field pos)]                                                                                                                                           (case dir
      \| (update pos :y #(+ dy %))
      \- (update pos :x #(+ dx %))
      \L (update (update pos :y #(+ % dx)) :x #(+ % dy))
      \J (update (update pos :y #(- % dx)) :x #(- % dy))
      \7 (update (update pos :y #(+ % dx)) :x #(+ % dy))
      \F (update (update pos :y #(- % dx)) :x #(- % dy))
      \S pos)))

(defn get-dir
  [last cur]
  [(- (get cur :x) (get last :x)) (- (get cur :y) (get last :y))])

(defn get-path
  [field path]
  (let [[dx dy] (get-dir (last (butlast path)) (last path))]
    (if (and (= 0 dx) (= 0 dy))
      path
      (recur field (conj path (get-next field (last path) dx dy))))))

(defn part-one
  ([] (part-one input-file-path))
  ([filename]
   (let [field (parse-input filename)
         start (get-start field)
         start-directions (get-potential-dirs field start)]
     ; remove 2 because last 2 are S again
     (/ (- (count (get-path field [start (first start-directions)])) 2) 2))))

(defn check-coord                                                                                                                                                          [field path start-face-north coord]
  (let [x-before (map (fn [x] {:x x :y (:y coord)}) (range (:x coord)))
        x-to-check (set/intersection (set path) (set x-before))]
    (if start-face-north
      (odd? (- (count x-to-check)
               (count (remove (set [\S \| \L \J]) (map #(field-get field %)
                                                       x-to-check)))))
      (odd? (- (count x-to-check)
               (count (remove (set [\| \L \J]) (map #(field-get field %)
                                                    x-to-check))))))))

(defn part-two
  ([] (part-two input-file-path))
  ([filename]
   (let [field (parse-input filename)
         start (get-start field)
         start-directions (get-potential-dirs field start)
         start-face-north (check-face-north field start)
         path (get-path field [start (first start-directions)])
         coords (remove (set path)
                        (apply concat
                               (map (fn [y] (map (fn [x] {:x x :y y})
                                                 (range (count (first field)))))
                                    (range (count field)))))]
     (count (filter true? (map #(check-coord field path start-face-north %)
                               coords))))))

(defn run 
  []
  (println (part-one))
  (println (part-two)))